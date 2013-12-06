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
        private static Environnement ENV_DEFAULT;
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
        private const int DISTANCECUBEACUBE = 1000;

        /// <summary>
        /// Distance minimale entre 2 maisons (au carré)
        /// </summary>
        private const int DISTANCEMAISONAMAISON = 50000;

        /// <summary>
        /// Coordonnée max exploitable 
        /// </summary>
        private const int MAXLONGUEURTERRAIN = 1000;

        private static Random rnd = new Random();
        #endregion

        #region Variables
        private int _UpdateForNaissance;

        /// <summary>
        /// Liste des personnages
        /// </summary>
        private Dictionary<String, Personnage> _ListPersonnages;

        /// <summary>
        /// Liste des cubes
        /// </summary>
        private Dictionary<String, Cube> _listCubes;
        private HashSet<String> _listCubesToDelete;

        /// <summary>
        /// Liste des maisons
        /// </summary>
        private Dictionary<String, Maison> _ListMaisons;
        private Mutex _mutMaison;

        /// <summary>
        /// Liste des tours
        /// </summary>
        private Dictionary<String, Maison> _ListTours;
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
        /// Piles FIFO pour la communication
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
        private Environnement(ref SceneManager scm)
        {
            _UpdateForNaissance = 0;
            _scm = scm;
            _ListPersonnages = new Dictionary<string, Personnage>();
            _ListMaisons = new Dictionary<string, Maison>();
            _mutMaison = new Mutex();
            _listCubes = new Dictionary<string, Cube>();
            _listCubesToDelete = new HashSet<string>();
            _ListOfComInput = new Queue<KnowledgeQuery>();
            _ListOfComOutput = new Queue<KnowledgeQuery>();
            _hsetCollisions = new HashSet<string>();

            //Abonnement au rafraichissement de la frame
            _fListener = new FrameListener.FrameStartedHandler(Update);
            Root.Singleton.FrameStarted += _fListener;
        }

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
            inc = 100;
            Vector3 vect = new Vector3(-350, 0, -1100);
            for (int i = 0; i < iNbOgres; i++)
            {
                vect = this.creer_vecteur(i, inc, vect);
                OgreOuvrier o = new OgreOuvrier(ref _scm, vect);
                _ListPersonnages.Add(o.NomEntity, o);
            }

            //Création des robots
            vect = new Vector3(-350, 0, 1100);
            for (int i = 0; i < iNbRobots; i++)
            {
                vect = this.creer_vecteur(i, inc, vect);
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
            } while (!ok);
            return v;
        }

        /// <summary>
        /// Renvoi un type de cube aléatoirement
        /// </summary>
        /// <returns>Type de cube</returns>
        private TypeCube getRandomTypeCube()
        {
            int z = rnd.Next(2);
            if (z == 0)
                return TypeCube.Bois;
            else
                return TypeCube.Pierre;
        }

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
        public static void createEnvironnement(ref SceneManager scm, int nbogres, int nbrobots, int nbcubes)
        {
            ENV_DEFAULT = new Environnement(ref scm);
            ENV_DEFAULT.initPersonnages(nbogres, nbrobots);
            ENV_DEFAULT.initCubes(nbcubes);

            //Démarre le thread
            ENV_DEFAULT._ComThread = new Thread(ENV_DEFAULT.ProcessComInOut);
            ENV_DEFAULT._stop = false;
            ENV_DEFAULT._ComThread.Start();
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
        /// <param name="min">Minimum en x et z</param>
        /// <param name="max">Maximum en x et z</param>
        /// <returns>Vecteur aléatoire</returns>
        public static Vector3 getRandomHorizontalVecteur()
        {
            int x = rnd.Next(-MAXLONGUEURTERRAIN, MAXLONGUEURTERRAIN+1);
            int y = 0;
            int z = rnd.Next(-MAXLONGUEURTERRAIN, MAXLONGUEURTERRAIN+1);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Retourne true selon la chance donnée
        /// </summary>
        /// <param name="nb">Une chance sur ce nombre de retourner true</param>
        /// <returns>True si chanceux, false sinon</returns>
        public bool getUneChanceSur(int nb)
        {
            int z = rnd.Next(nb);
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

            //fini les threads de tous les personnages
            foreach (KeyValuePair<String, Personnage> kvp in _ListPersonnages)
            {
                kvp.Value.Dispose();
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
            if ((_listCubes.ContainsKey(nomCube))&&(!_listCubesToDelete.Contains(nomCube)))
            {
                _listCubes.TryGetValue(nomCube, out cubeOut);
                //Enlève le cube de la liste
                if (cubeOut != null)
                    _listCubesToDelete.Add(nomCube);
            }
            return cubeOut;
        }

        /// <summary>
        /// Donne le cube à la maison
        /// </summary>
        /// <param name="c">Cube donné</param>
        /// <param name="nomMaison">Nom de la maison</param>
        /// <returns>True si le cube a été transféré, false sinon</returns>
        public bool giveCube(Cube c, String nomMaison)
        {
            bool isOk = false;

            Maison m = null;
            if (_ListMaisons.ContainsKey(nomMaison))
            {
                _ListMaisons.TryGetValue(nomMaison, out m);
                //Ajoute le cube à la maison
                if (m != null)
                    isOk = m.ajoutDeBloc(c);
            }

            return isOk;
        }
        #endregion

        #region Méthodes privées
        /// <summary>
        /// méthode d'ajout d'un ogre dans l'env via les maisons
        /// </summary>
        /// <param name="typeOgre">type de l'ogre a créer</param>
        /// <param name="position">endroit ou il apparait</param>
        private void AddOgreToEnv(Type typeOgre, Vector3 position)
        {
            if (typeOgre == typeof(OgreOuvrier))
            {
                OgreOuvrier o = new OgreOuvrier(ref _scm, position);
                _ListPersonnages.Add(o.NomEntity, o);
            }
            else
            {
                OgreBatisseur o = new OgreBatisseur(ref _scm, position);
                _ListPersonnages.Add(o.NomEntity, o);
            }

        }

        /// <summary>
        /// Appelé à la MAJ de la frame
        /// </summary>
        /// <param name="fEvt"></param>
        /// <returns></returns>
        private bool Update(FrameEvent fEvt)
        {
            if( _UpdateForNaissance >= 2000)
            {
                foreach(KeyValuePair<String, Maison> kvpMaison in _ListMaisons)
                {
                    this.AddOgreToEnv(kvpMaison.Value.NaissanceOgre(), kvpMaison.Value.Position);
                }
                _UpdateForNaissance = 0;
            }
            else
            {
                _UpdateForNaissance++;
            }

            //Regarde s'il y a des cubes à renouveler
            while (_listCubesToDelete.Count != 0)
            {
                //Enlève le cube pris par l'ogre
                _listCubes.Remove(_listCubesToDelete.First());
                _listCubesToDelete.Remove(_listCubesToDelete.First());

                //Ajoute un cube sur le terrain
                Cube c = new Cube(ref _scm, getPositionAntiCollisionCube(DISTANCECUBEACUBE), getRandomTypeCube());
                _listCubes.Add(c.NomEntity, c);
            }

            //Detection des collisions avec les personnages
            int iPerso = 1;
            KeyValuePair<String, Personnage>[] tabPerso = _ListPersonnages.ToArray();
            foreach (KeyValuePair<String, Personnage> kvpPerso in _ListPersonnages)
            {                
                //Détection des collisions avec les cubes
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
                    _ListPersonnages[tabPerso[i].Key].Dispose();
                    _ListPersonnages.Remove(tabPerso[i].Key);
                }
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
            //Demande de maison
            if (iKQ.Classe == Classe.Maison)
            {
                //Vérification
                bool ok = true;
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
                    _mutMaison.WaitOne();
                    _ListMaisons.Add(m.NomEntity, m);
                    _mutMaison.ReleaseMutex();
                }
            }
        }
        #endregion
    }
}
