using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreFramework;
using System.Threading;
using System.Collections;

namespace BaseMogre
{
    class Environnement : IComKnowledgeQuery, IDisposable
    {
        #region Variables Statiques
        /// <summary>
        /// environnement de base
        /// </summary>
        private static Environnement ENV_DEFAULT;

        /// <summary>
        /// base du nom de l'environnement
        /// </summary>
        private const String ENV_NAME = "Environnement";

        /// <summary>
        /// Distance pour la detection d'une collision avec un cube (au carré)
        /// </summary>
        private const int DISTANCECOLLISIONCUBE = 1500;

        /// <summary>
        /// Distance pour la detection d'une collision avec un perso (au carré)
        /// </summary>
        private const int DISTANCECOLLISIONPERSO = 2000;

        /// <summary>
        /// Distance pour la detection d'une collision avec une maison (au carré)
        /// </summary>
        private const int DISTANCECOLLISIONMAISON = 10000;

        /// <summary>
        /// Distance minimale entre 2 cubes (au carré)
        /// </summary>
        private const int DISTANCECUBEACUBE = 10000;

        /// <summary>
        /// Distance minimale entre 2 maisons (au carré)
        /// </summary>
        private const int DISTANCEMAISONAMAISON = 50000;

        /// <summary>
        /// Coordonnée max exploitable 
        /// </summary>
        private const int MAXLONGUEURTERRAIN = 1000;

        /// <summary>
        /// Ecart MIN-MAX fait lors d'un déplacement, d'une collision, etc
        /// </summary>
        private const int ECARTMIN = 50;
        private const int ECARTMAX = 200;

        /// <summary>
        /// Objet pour les random de la classe
        /// </summary>
        private static Random rnd = new Random();
        #endregion
        
        #region Variables
        /// <summary>
        /// caméra du monde
        /// </summary>
        private Camera cam;

        /// <summary>
        /// nombre de secondes depuis la derniere naissance
        /// </summary>
        private float _UpdateForNaissance;

        /// <summary>
        /// Liste des personnages
        /// </summary>
        private Dictionary<String, Personnage> _ListPersonnages;

        /// <summary>
        /// Liste des cubes et mutex associé
        /// </summary>
        private Dictionary<String, Cube> _listCubes;
        private Mutex _mutCubes;

        /// <summary>
        /// Liste des maisons et mutex associé
        /// </summary>
        private Dictionary<String, Maison> _ListMaisons;
        private Mutex _mutMaison;

        /// <summary>
        /// Tour unique et mutex associé
        /// </summary>
        private Tour _tour;
        private Mutex _mutTours;

        /// <summary>
        /// HashSet pour les collisions
        /// </summary>
        private HashSet<String> _hsetCollisions;

        /// <summary>
        /// Scenemanager
        /// </summary>
        private SceneManager _scm;

        /// <summary>
        /// Piles FIFO pour la communication IN et OUT
        /// </summary>
        private Queue<KnowledgeQuery> _ListOfComInput;
        private Queue<KnowledgeQuery> _ListOfComOutput;

        /// <summary>
        /// Thread qui traite les communications
        /// </summary>
        private Thread _ComThread;

        /// <summary>
        /// Booleen pour la gestion du thread de com
        /// </summary>
        private volatile bool _stop;

        /// <summary>
        /// Listener pour le raffraichissement des frames
        /// </summary>
        private FrameListener.FrameStartedHandler _fListener;

        /// <summary>
        /// Si l'un des camp à gagné
        /// </summary>
        private bool _isFini;
        #endregion

        #region Evenement
        public delegate void FinishEventHandler(String message);

        /// <summary>
        /// Evenement qui se déclenche à la fin 
        /// </summary>
        public static event FinishEventHandler FinishEvent;
        #endregion

        #region Getter et Setter
        public Vector3 PositionEntrepotPierre
        {
            //TODO
            get { return new Vector3(); }
        }
        public Vector3 PositionEntrepotBois
        {
            //TODO
            get { return new Vector3(); }
        }
        #endregion

        #region Constructeur/Destructeur
        private Environnement(ref SceneManager scm, ref Camera cam)
        {
            //initialisation de l'environnement
            this.cam = cam;
            _UpdateForNaissance = 0;
            _scm = scm;
            _isFini = false;
            _ListPersonnages = new Dictionary<string, Personnage>();
            _ListMaisons = new Dictionary<string, Maison>();
            _mutMaison = new Mutex();
            _mutTours = new Mutex();
            _listCubes = new Dictionary<string, Cube>();
            _mutCubes = new Mutex();
            _ListOfComInput = new Queue<KnowledgeQuery>();
            _ListOfComOutput = new Queue<KnowledgeQuery>();
            _hsetCollisions = new HashSet<string>();

            //Abonnement au rafraichissement de la frame
            _fListener = new FrameListener.FrameStartedHandler(Update);
            Root.Singleton.FrameStarted += _fListener;
        }

        /// <summary>
        /// Destructeur
        /// </summary>
        ~Environnement()
        {
            //fini le thread
            _stop = true;
            _ComThread.Join();
        }
        #endregion

        #region Méthodes d'initialisation
        /// <summary>
        /// Initialisation des personnages
        /// </summary>
        /// <param name="iNbOgres">Nombre d'ogres ouvriers</param>
        /// <param name="iNbRobots">Nombre de robots</param>
        private void initPersonnages(int iNbOgres, int iNbRobots)
        {
            int inc;

            //Création des ogres
            inc = 200;
            Vector3 vect = new Vector3(-1100, 0, -1100);
            for (int i = 0; i < iNbOgres; i++)
            {
                vect = this.creer_vecteur(i, inc, vect);
                OgreOuvrier o = new OgreOuvrier(ref _scm, vect);
                _ListPersonnages.Add(o.NomEntity, o);
            }

            //Création des robots
            vect = new Vector3(1100, Map.ALIGNEMENTTERRAIN, 1100);
            for (int i = 0; i < iNbRobots; i++)
            {
                vect = this.creer_vecteur(i, -inc, vect);
                Robot r = new Robot(ref _scm, vect);
                _ListPersonnages.Add(r.NomEntity, r);
            }
        }

        /// <summary>
        /// Initialisation des cubes de départ
        /// </summary>
        /// <param name="nbCubes">Nombre de cubes</param>
        private void initCubes(int nbCubes)
        {
            Vector3 vec = Vector3.ZERO;
            for (int i = 0; i < nbCubes; i++)
            {
                TypeCube t = getRandomTypeCube();

                vec = getPositionAntiCollisionCube(DISTANCECUBEACUBE);
                Cube c = new Cube(ref _scm, vec, t);
                _listCubes.Add(c.NomEntity, c);
            }
        }

        /// <summary>
        /// Obtient une position qui laisse une marge minimale
        /// </summary>
        /// <returns>Vecteur</returns>
        private Vector3 getPositionAntiCollisionCube(int marge)
        {
            bool ok;
            Vector3 v;
            do
            {
                ok = true;
                v = getRandomHorizontalVecteur();
                foreach (KeyValuePair<String, Cube> kvp in _listCubes)
                {
                    float distanceAuCarre = (kvp.Value.Position - v).SquaredLength;
                    if (distanceAuCarre < marge)
                        ok = false;
                }
                _mutMaison.WaitOne();
                foreach (KeyValuePair<String, Maison> kvp in _ListMaisons)
                {
                    float distanceAuCarre = (kvp.Value.Position - v).SquaredLength;
                    if (distanceAuCarre < marge)
                        ok = false;
                }
                _mutMaison.ReleaseMutex();
            } while (!ok);
            return v;
        }

        /// <summary>
        /// Renvoi un type de cube aléatoirement
        /// </summary>
        /// <returns>Type de cube</returns>
        private TypeCube getRandomTypeCube()
        {
            int z;
            lock (rnd) z=rnd.Next(2);
            if (z == 0)
                return TypeCube.Bois;
            else
                return TypeCube.Pierre;
        }

        /// <summary>
        /// génération d'un vecteur relatif à un précédent pour le placement en ligne
        /// </summary>
        /// <param name="i">numéro de l'objet</param>
        /// <param name="inc">décalage à ajouter</param>
        /// <param name="ancien">vecteur du précédent objet</param>
        /// <returns>vecteur suivant</returns>
        private Vector3 creer_vecteur(int i, int inc, Vector3 ancien)
        {
            if ((i % 10 == 0) && (i != 0))
            {
                ancien.z += inc;
                ancien.x += -inc * 9;
            }
            else
            {
                ancien.x += inc;
            }
            return ancien;
        }
        #endregion

        #region Méthodes Statiques
        /// <summary>
        /// Crée l'instance de l'environnement
        /// </summary>
        /// <param name="scm">SceneManager de référence</param>
        /// <param name="nbogres">Nombre d'ogres ouvriers</param>
        /// <param name="nbrobots">Nombre de robots</param>
        public static void createEnvironnement(ref SceneManager scm, int nbogres, int nbrobots, int nbcubes,ref Camera cam)
        {
            ENV_DEFAULT = new Environnement(ref scm, ref cam);
            ENV_DEFAULT.initPersonnages(nbogres, nbrobots);
            ENV_DEFAULT.initCubes(nbcubes);

            //Démarre le thread
            ENV_DEFAULT._ComThread = new Thread(ENV_DEFAULT.ProcessComInOut);
            ENV_DEFAULT._stop = false;
            ENV_DEFAULT._ComThread.Start();

            Log.writeNewLine("Simulation commencée !");
        }

        /// <summary>
        /// Retourne l'instance courante d'Environnement
        /// </summary>
        /// <returns></returns>
        public static Environnement getInstance()
        {
            return ENV_DEFAULT;
        }

        /// <summary>
        /// Renvoi un vecteur aléatoire sur le plan horizontal
        /// </summary>
        /// <returns>Vecteur aléatoire</returns>
        public static Vector3 getRandomHorizontalVecteur()
        {
            int x, y, z;
            lock (rnd)
            {
                x = rnd.Next(-MAXLONGUEURTERRAIN, MAXLONGUEURTERRAIN + 1);
                y = 0;
                z = rnd.Next(-MAXLONGUEURTERRAIN, MAXLONGUEURTERRAIN + 1);
            }
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Destination aléatoire
        /// </summary>
        /// <param name="pos">Position de départ (pour ne pas dépasser les limites du terrain)</param>
        /// <returns>Nouvelle Destination</returns>
        public static Vector3 getRandomDestination(Vector3 pos)
        {
            Vector3 nPos;
            int nEssais = 10;
            do
            {
                float x, y, z;
                lock (rnd)
                {
                    x = rnd.Next(-45, 46);
                    y = 0;
                    z = rnd.Next(-45, 46);
                }
                Vector3 dir = new Vector3(x, y, z);
                dir.Normalise();

                int lenght = getRandomEcart();
                nPos = pos + (dir * lenght);

                nEssais--;
            } while ((System.Math.Max(System.Math.Abs(nPos.x), System.Math.Abs(nPos.z))>MAXLONGUEURTERRAIN)&&(nEssais>0));

            if (nEssais <= 0)
                nPos = getRandomHorizontalVecteur();

            return nPos;
        }

        /// <summary>
        /// Fourni un ecart à multiplier par un vecteur de direction
        /// </summary>
        /// <returns></returns>
        public static int getRandomEcart()
        {
            int ecart;
            lock (rnd) ecart = rnd.Next(ECARTMIN, ECARTMAX);
            return ecart;
        }

        /// <summary>
        /// Retourne true selon la chance donnée
        /// </summary>
        /// <param name="nb">Une chance sur ce nombre de retourner true</param>
        /// <returns>True si chanceux, false sinon</returns>
        public bool getUneChanceSur(int nb)
        {
            int z;
            lock (rnd) z = rnd.Next(nb);
            if (z < 1)
                return true;
            else
                return false;
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Envoi un message à l'environnement
        /// </summary>
        /// <param name="iKQ">Requète de communication</param>
        /// <returns>Valeur si la communication s'est bien déroulée</returns>
        public Result send(KnowledgeQuery iKQ)
        {
            if (iKQ.NomPerso != null)
            {
                _ListOfComInput.Enqueue(iKQ);
                return Result.OK;
            }
            else
            {
                return Result.FAIL;
            }
        }

        /// <summary>
        /// Fini le thread de traitement des messages de l'environnement
        /// </summary>
        public void Dispose()
        {
            //fini le thread
            _stop = true;
            _ComThread.Join();

            //Efface le message
            FinishEvent("");

            //Arrêt du listener
            Root.Singleton.FrameStarted -= _fListener;

            //fini les threads de tous les personnages
            KeyValuePair<String, Personnage>[] tabPerso = _ListPersonnages.ToArray();
            for (int i = 0; i < tabPerso.Length; i++)
            {
                _ListPersonnages.Remove(tabPerso[i].Key);
                tabPerso[i].Value.Dispose();
            }

            //supprime les cubes
            KeyValuePair<String, Cube>[] tabCubes = _listCubes.ToArray();
            for (int i = 0; i < tabCubes.Length; i++)
            {
                _listCubes.Remove(tabCubes[i].Key);
                tabCubes[i].Value.Dispose();
            }

            //supprime les maisons
            KeyValuePair<String, Maison>[] tabMaisons = _ListMaisons.ToArray();
            for (int i = 0; i < tabMaisons.Length; i++)
            {
                _ListMaisons.Remove(tabMaisons[i].Key);
                tabMaisons[i].Value.Dispose();
            }

            //supprime la tour
            if (_tour != null)
            {
                _tour.Dispose();
                _tour = null;
            }
        }

        /// <summary>
        /// Obtient le cube à partir du nom
        /// </summary>
        /// <param name="nomCube">Nom du cube</param>
        /// <returns>Le cube ou null si introuvable</returns>
        public Cube getCube(String nomCube)
        {
            Cube cubeOut = null;
            if (_listCubes.ContainsKey(nomCube))
            {
                _listCubes.TryGetValue(nomCube, out cubeOut);
                //Enlève le cube de la liste
                if (cubeOut != null)
                {
                    _mutCubes.WaitOne();
                    _listCubes.Remove(nomCube);

                    //Ajoute un cube sur le terrain
                    Cube c = new Cube(ref _scm, getPositionAntiCollisionCube(DISTANCECUBEACUBE), getRandomTypeCube());
                    _listCubes.Add(c.NomEntity, c);
                    _mutCubes.ReleaseMutex();
                }
            }
            return cubeOut;
        }

        /// <summary>
        /// Donne le cube à un bâtiment
        /// </summary>
        /// <param name="c">Cube donné</param>
        /// <param name="nomMaison">Nom du bâtiment</param>
        /// <returns>True si le cube a été transféré, false sinon</returns>
        public bool giveCube(Cube c, String nomBatiment)
        {
            bool isOk = false;

            Maison m = null;
            if (_ListMaisons.ContainsKey(nomBatiment))
            {
                _ListMaisons.TryGetValue(nomBatiment, out m);
                //Ajoute le cube à la maison
                if (m != null)
                    isOk = m.ajoutDeBloc(c);
            }
            else if (_tour.NomEntity == nomBatiment)
            {
                isOk = _tour.ajoutDeBloc(c);
                //Condition de victoire des ogres
                if ((_tour.isFinish())&&(!_isFini))
                {
                    Log.writeNewLine("Les ogres ont gagnés !");
                    _isFini = true;
                    FinishEvent("Les ogres ont gagnés !");
                }
            }

            return isOk;
        }

        /// <summary>
        /// gestion de la camera si elle est attachée
        /// </summary>
        /// <param name="c">caméra qui doit être suivie</param>
        /// <param name="indice">indice correspondant à l'objet que l'on veux suivre</param>
        public bool attachedCamera(int indice)
        {
            bool fin = false;
            cam.SetPosition(cam.Position.x + 2, cam.Position.y, cam.Position.z);
            if (indice > _ListPersonnages.Count)
            {
                indice = _ListPersonnages.Count - 1;
                fin = true;
            }
            foreach (KeyValuePair<String, Personnage> kvpPerso in _ListPersonnages)
            {
                if(indice == 0)
                {
                    cam.Position = kvpPerso.Value.Position + new Vector3(300, 300, 300) ;
                    cam.LookAt(kvpPerso.Value.Position);
                    break;
                }
                else
                {
                    indice--;
                }
            }
            return fin;
            
        }
        #endregion

        #region Méthodes privées
        /// <summary>
        /// méthode d'ajout d'un ogre dans l'env via les maisons
        /// </summary>
        /// <param name="type">type de l'ogre a créer</param>
        /// <param name="position">endroit ou il apparait</param>
        private void AddPersoToEnv(Type type, Vector3 position)
        {
            if (type == typeof(Robot))
            {
                Robot r = new Robot(ref _scm, position);
                _ListPersonnages.Add(r.NomEntity, r);
                Log.writeNewLine("Nouveau robot créé :" + r.NomEntity);
            }
            else if (type == typeof(OgreOuvrier))
            {
                OgreOuvrier o = new OgreOuvrier(ref _scm, position);
                _ListPersonnages.Add(o.NomEntity, o);
                Log.writeNewLine("Nouvel ogre ouvrier créé :" + o.NomEntity);
            }
            else
            {
                OgreBatisseur o;
                if (_tour != null)
                    o = new OgreBatisseur(ref _scm, position, _tour.getInfo());
                else
                    o = new OgreBatisseur(ref _scm, position);
                _ListPersonnages.Add(o.NomEntity, o);
                Log.writeNewLine("Nouvel ogre batisseur créé : " + o.NomEntity);
            }

        }

        /// <summary>
        /// Appelé à la MAJ de la frame
        /// </summary>
        /// <param name="fEvt"></param>
        /// <returns></returns>
        private bool Update(FrameEvent fEvt)
        {
            //fait naitre les nouveau personnages
            if (!_isFini)
            {
                if (_UpdateForNaissance >= Variables.TEMPSDAPPARITION)
                {
                    _mutMaison.WaitOne();
                    foreach (KeyValuePair<String, Maison> kvpMaison in _ListMaisons)
                    {
                        if (kvpMaison.Value.isFinish())
                            this.AddPersoToEnv(kvpMaison.Value.NaissancePerso(), kvpMaison.Value.Position);
                    }
                    _mutMaison.ReleaseMutex();
                    _UpdateForNaissance = 0;
                }
                else
                {
                    _UpdateForNaissance += fEvt.timeSinceLastFrame;
                }
            }

            //Detection des collisions avec les personnages
            int iPerso = 1;
            bool fini = true;
            KeyValuePair<String, Personnage>[] tabPerso = _ListPersonnages.ToArray();
            foreach (KeyValuePair<String, Personnage> kvpPerso in _ListPersonnages)
            {
                if (kvpPerso.Value.getClasse() == Classe.Ogre)
                    fini = false;

                //Détection des collisions avec les cubes
                _mutCubes.WaitOne();
                foreach (KeyValuePair<String, Cube> kvpCube in _listCubes)
                {
                    float distanceAuCarre = (kvpPerso.Value.Position - kvpCube.Value.Position).SquaredLength;
                    string name = kvpPerso.Key + kvpCube.Key;
                    if (distanceAuCarre < DISTANCECOLLISIONCUBE)
                    {
                        if (!_hsetCollisions.Contains(name))
                        {
                            //Collision
                            _hsetCollisions.Add(name);

                            //Message pour l'ogre
                            KnowledgeQuery kq = new KnowledgeQuery(kvpPerso.Key, Classe.Cube, kvpCube.Key, kvpCube.Value.Position);
                            _ListOfComOutput.Enqueue(kq);         
                        }
                    }
                    else if (_hsetCollisions.Contains(name))
                    {
                        _hsetCollisions.Remove(name);
                    }
                }
                _mutCubes.ReleaseMutex();

                //Detection des collisions avec les maisons
                _mutMaison.WaitOne();
                foreach (KeyValuePair<String, Maison> kvpMaison in _ListMaisons)
                {
                    float distanceAuCarre = (kvpPerso.Value.Position - kvpMaison.Value.Position).SquaredLength;
                    string name = kvpPerso.Key + kvpMaison.Key;
                    if (distanceAuCarre < DISTANCECOLLISIONMAISON)
                    {
                        if (!_hsetCollisions.Contains(name))
                        {
                            //Collision
                            _hsetCollisions.Add(name);

                            //Message
                            KnowledgeQuery kq = new KnowledgeQuery(kvpPerso.Key, Classe.Maison, kvpMaison.Key, kvpMaison.Value.Position, kvpMaison.Value.isFinish().ToString());
                            _ListOfComOutput.Enqueue(kq);
                        }
                    }
                    else if (_hsetCollisions.Contains(name))
                    {
                        _hsetCollisions.Remove(name);
                    }
                }
                _mutMaison.ReleaseMutex();

                //Collision tour
                _mutTours.WaitOne();
                if (_tour != null)
                {
                    float distanceAuCarre = (kvpPerso.Value.Position - _tour.Position).SquaredLength;
                    string name = kvpPerso.Key + _tour.NomEntity;
                    if (distanceAuCarre < DISTANCECOLLISIONMAISON)
                    {
                        if (!_hsetCollisions.Contains(name))
                        {
                            //Collision
                            _hsetCollisions.Add(name);

                            //Message
                            KnowledgeQuery kq = new KnowledgeQuery(kvpPerso.Key, Classe.Tour, _tour.NomEntity, _tour.Position, _tour.isFinish().ToString());
                            _ListOfComOutput.Enqueue(kq);
                        }
                    }
                    else if (_hsetCollisions.Contains(name))
                    {
                        _hsetCollisions.Remove(name);
                    }
                }
                _mutTours.ReleaseMutex();

                //Détection des collisions avec les autres persos
                for (int i = iPerso; i < tabPerso.Length; i++)
                {
                    float distanceAuCarre = (kvpPerso.Value.Position - tabPerso[i].Value.Position).SquaredLength;
                    string name = kvpPerso.Key + tabPerso[i].Key;
                    if (distanceAuCarre < DISTANCECOLLISIONPERSO)
                    {
                        if (!_hsetCollisions.Contains(name))
                        {
                            //Collision
                            _hsetCollisions.Add(name);

                            //Messages
                            KnowledgeQuery kq = new KnowledgeQuery(kvpPerso.Key, tabPerso[i].Value.getClasse(), tabPerso[i].Key, tabPerso[i].Value.Position, tabPerso[i].Value.Attaque.ToString());
                            KnowledgeQuery kq2 = new KnowledgeQuery(tabPerso[i].Key, kvpPerso.Value.getClasse(), kvpPerso.Key, kvpPerso.Value.Position, kvpPerso.Value.Attaque.ToString());
                            _ListOfComOutput.Enqueue(kq);
                            _ListOfComOutput.Enqueue(kq2);
                        }
                    }
                    else if (_hsetCollisions.Contains(name))
                    {
                        _hsetCollisions.Remove(name);
                    }
                }

                iPerso++;
            }

            //Detection des persos morts
            for (int i = 0; i < tabPerso.Length; i++)
            {
                //Suppression
                if (!tabPerso[i].Value.isAlive())
                {
                    Log.writeNewLine("Le personnage : " + tabPerso[i].Value.NomEntity + " est mort.");
                    _ListPersonnages[tabPerso[i].Key].Dispose();
                    _ListPersonnages.Remove(tabPerso[i].Key);
                }
            }

            //Si tout les ogres ont été tués
            if ((fini)&&(!_isFini))
            {
                Log.writeNewLine("Les robots ont gagnés !");
                _isFini = true;
                FinishEvent("Les robots ont gagnés !");
            }

            return true;
        }

        /// <summary>
        /// Traite les communications dans un thread à part
        /// </summary>
        private void ProcessComInOut()
        {
            while (!_stop)
            {
                //Reception
                while ((_ListOfComInput.Count > 0)&&(!_stop))
                {
                    KnowledgeQuery kq = _ListOfComInput.Dequeue();
                    ProcessKQ(kq);
                }

                //Envoi
                while ((_ListOfComOutput.Count > 0)&&(!_stop))
                {
                    KnowledgeQuery kq = _ListOfComOutput.Dequeue();
                    if (kq.NomPerso != null)
                    {
                        if (_ListPersonnages.ContainsKey(kq.NomPerso))
                        {
                            Personnage p;
                            _ListPersonnages.TryGetValue(kq.NomPerso, out p);
                            if (p!=null)
                                p.send(kq);
                        }
                    }
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Traite la knowledge query
        /// </summary>
        /// <param name="iKQ"></param>
        private void ProcessKQ(KnowledgeQuery iKQ)
        {
            //Transmet une information
            if (iKQ.Parametre == "info")
            {
                KnowledgeQuery kq = new KnowledgeQuery(iKQ.NomPerso, iKQ.Classe, iKQ.Nom, iKQ.Position, iKQ.Parametre);
                _ListOfComOutput.Enqueue(kq);
            }
            else
            {
                //Demande de maison
                if (iKQ.Classe == Classe.Maison)
                {
                    //Vérification
                    bool ok = true;
                    _mutMaison.WaitOne();
                    foreach (KeyValuePair<String, Maison> kvp in _ListMaisons)
                    {
                        float distanceAuCarre = (kvp.Value.Position - iKQ.Position).SquaredLength;
                        if (distanceAuCarre < DISTANCEMAISONAMAISON)
                            ok = false;
                    }

                    //Création
                    if (ok)
                    {
                        Maison m = new Maison(ref _scm, iKQ.Position);
                        _ListMaisons.Add(m.NomEntity, m);
                    }
                    _mutMaison.ReleaseMutex();
                }
                else if ((iKQ.Classe == Classe.Tour) && (_tour == null))
                {
                    //Vérification
                    bool ok = true;
                    _mutMaison.WaitOne();
                    foreach (KeyValuePair<String, Maison> kvp in _ListMaisons)
                    {
                        float distanceAuCarre = (kvp.Value.Position - iKQ.Position).SquaredLength;
                        if (distanceAuCarre < DISTANCEMAISONAMAISON)
                            ok = false;
                    }
                    _mutMaison.ReleaseMutex();

                    //Création
                    if (ok)
                    {
                        _mutTours.WaitOne();
                        _tour = new Tour(ref _scm, iKQ.Position);
                        _mutTours.ReleaseMutex();
                    }
                }
            }
        }
        #endregion
    }
}
