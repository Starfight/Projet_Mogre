using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mogre;

namespace BaseMogre
{
    abstract class Personnage : IComKnowledgeQuery, IDisposable
    {
        #region Constantes
        /// <summary>
        /// Nom du node par défaut
        /// </summary>
        private const String NAMENODE = "Node_";

        /// <summary>
        /// Distance pour esquiver une collision
        /// </summary>
        protected const int ESQUIVE_COLLISION = 200;
        #endregion

        #region Variables
        /// <summary>
        /// Points de vie 
        /// </summary>
        protected int _pointsDeVie;

        /// <summary>
        /// Référence du scenemanager
        /// </summary>
        protected SceneManager _scm;

        /// <summary>
        /// Entity représentant le personnage
        /// </summary>
        protected Entity _entity;

        /// <summary>
        /// Scenenode du personnage
        /// </summary>
        protected SceneNode _node;

        /// <summary>
        /// Nom du personnage
        /// </summary>
        protected String _nomEntity;

        /// <summary>
        /// Thread pour le fonctionnement autonome du personnage
        /// </summary>
        private Thread _threadMission;

        /// <summary>
        /// Indique si la prise de décision doit se lancer
        /// </summary>
        protected volatile bool _needToDecide;

        /// <summary>
        /// Booleen pour stopper les threads
        /// </summary>
        protected volatile bool _stop;

        /// <summary>
        /// Destination en cours d'acheminement
        /// </summary>
        private Vector3 _destination;

        /// <summary>
        /// Vecteur normé pour la direction
        /// </summary>
        protected Vector3 _vDirection;

        /// <summary>
        /// Distance à parcourir
        /// </summary>
        protected double _distance;

        /// <summary>
        /// Indique des modification à effectuer
        /// </summary>
        protected bool _DestinationChanged;

        /// <summary>
        /// Listener pour le raffraichissement des frames
        /// </summary>
        private FrameListener.FrameStartedHandler _fListener;

        /// <summary>
        /// Liste des objets possédés
        /// </summary>
        //private List<Objet> _objetsPerso;

        /// <summary>
        /// Queue pour la communication
        /// </summary>
        protected Queue<KnowledgeQuery> _listComInput;
        #endregion

        #region Constructeur
        public Personnage(ref SceneManager scm, Vector3 position, String nomPersonnage, String nomMesh)
        {
            //Création de l'Entity et du Scenenode à la position
            _entity = scm.CreateEntity(nomPersonnage, nomMesh);
            _node = scm.RootSceneNode.CreateChildSceneNode(NAMENODE + nomPersonnage, position);
            _node.AttachObject(_entity);

            //Enregistrement du Scenemanager
            _scm = scm;

            //Enregistrement du nom du personnage
            _nomEntity = nomPersonnage;

            //Définition de la destination initiale
            _destination = this.Position;
            _DestinationChanged = false;
            _vDirection = new Vector3(1, 0, 0);
            _distance = 0;

            //Abonnement au rafraichissement de la frame
            _fListener = new FrameListener.FrameStartedHandler(Update);
            Root.Singleton.FrameStarted += _fListener;

            //Queue pour la com
            _listComInput = new Queue<KnowledgeQuery>();

            //Prise de décision
            _needToDecide = true;
            //Démarage du thread
            _threadMission = new Thread(Start);
            _stop = false;
            _threadMission.Start();
        }
        public Personnage() { }
        #endregion

        #region Getters et Setters
        /// <summary>
        /// Nom de l'entité
        /// </summary>
        public String NomEntity
        {
            get { return _nomEntity; }
        }
        /// <summary>
        /// Position du personnage
        /// </summary>
        public Vector3 Position
        {
            get { return _node.Position; }
        }
        /// <summary>
        /// Destination du personnage
        /// </summary>
        protected Vector3 Destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                //Indique des modification à faire 
                _DestinationChanged = true;
            }
        }

        //public List<Objet> ObjetsPerso
        //{
        //  get { return _objetsPerso; }
        //  set { _objetsPerso = value; }
        //}
        #endregion

        #region methodes publiques
        public Result send(KnowledgeQuery iKQ)
        {
            if (iKQ.NomPerso != null)
            {
                _listComInput.Enqueue(iKQ);
                _needToDecide = true;
                return Result.OK;
            }
            else
            {
                return Result.FAIL;
            }
        }

        /// <summary>
        /// Implémentation de l'interface IDisposable pour l'arrêt des threads
        /// </summary>
        public void Dispose()
        {
            //Stoppe le thread
            _stop = true;
            if (_threadMission.ThreadState == ThreadState.Running)
                _threadMission.Join();
        }

        /* méthodes pour l'inventaire
         *A voir pour la supression
        /// <summary>
        /// méthodes de recherche dans l'inventaire
        /// </summary>
        /// <param name="t">type d'objet recherché</param>
        /// <returns>si ou ou non il posséde un objet</returns>
        public bool possedeObjet(Type t)
        {
            foreach(Objet obj in this._objetsPerso)
            {
                if(t == obj.GetType())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de récuperer un objet dans l'inventaire
        /// </summary>
        /// <param name="t">type de l'objet voulu</param>
        /// <returns>null ou l'objet voulu</returns>
        public Objet getObjet(Type t)
        {
            Objet O = null;
            foreach (Objet obj in this._objetsPerso)
            {
                if (t == obj.GetType())
                {
                    O= obj;
                    break;
                }
            }
            if (O != null)
                this._objetsPerso.Remove(O);
            return O;
        }

        /// <summary>
        /// ajout d'un objet à l'inventaire
        /// </summary>
        /// <param name="o">objet à ajouter</param>
        public void addObjet(Objet o)
        {
            if (o != null)
                this._objetsPerso.Add(o);
        }
         */
        #endregion

        #region Methodes abstract
        /// <summary>
        /// Méthode permettant la prise de décision
        /// </summary>
        protected abstract void Decision();

        /// <summary>
        /// Méthode permettant de mettre à jour le personnage dans le monde 3D
        /// </summary>
        /// <param name="fEvt">Fourni des information sur le raffraichissement des images</param>
        /// <returns>False si l'update devrait être stopée, vrai autrement</returns>
        protected abstract bool Update(FrameEvent fEvt);

        /// <summary>
        /// Retourne la classe du personnage
        /// </summary>
        /// <returns></returns>
        public abstract Classe getClasse();
        #endregion

        #region Méthodes privées
        /// <summary>
        /// Raffraichit la prise de décision
        /// </summary>
        private void Start()
        {
            while (!_stop)
            {
                if (_needToDecide)
                {
                    Decision();

                    //indique que la décision a bien été prise
                    _needToDecide = false;
                }
                else
                {
                    Thread.Sleep(20);
                }
            }
        }
        #endregion

        #region Méthodes protected
        /// <summary>
        /// Evite la collision avec un objet
        /// </summary>
        /// <param name="pos">Position de l'objet en collision</param>
        protected void EviteCollision(Vector3 pos)
        {
            Vector3 v = (Position - pos);
            v.Normalise();
            Destination = v * ESQUIVE_COLLISION;
        }
        #endregion
    }
}
