using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Threading;

namespace BaseMogre
{
    class Maison:Batiment
    {
        #region Constantes/Variables statiques
        /// <summary>
        /// Compteur de maison
        /// </summary>
        private static int _COUNT = 0;

        /// <summary>
        /// Nom de base
        /// </summary>
        private static String NAMEDEFAULT = "maison";

        private static Random RND = new Random();
        #endregion

        #region variables
        private Mutex _depotCube;

        private PositionCubes _positionFuture;

        private SceneNode _nodeBaseMaison;
        #endregion

        public Maison(ref SceneManager scm,Vector3 position)
            : base(ref scm, position, NAMEDEFAULT + _COUNT, 20,11,12)
        {
            Entity entity = scm.CreateEntity("base_maison" + _COUNT, "cube.mesh");
            _nodeBaseMaison = scm.RootSceneNode.CreateChildSceneNode("base_maison_node" + _COUNT, position);
            _nodeBaseMaison.SetPosition(position.x, position.y - 12, position.z);
            _nodeBaseMaison.Scale(new Vector3(1, 0.05f, 1));
            _nodeBaseMaison.AttachObject(entity);
            entity.SetMaterialName("Texture/BaseMaison");
           
            _COUNT++;
            _positionFuture = new PositionCubes(this.Position.x+30, 0, this.Position.z-30);
            _depotCube = new Mutex();
        }

        public bool ajoutDeBloc(Cube C)
        {
            _depotCube.WaitOne();
            bool possible = this.ajoutCube(C);
            if (possible)
            {
                C.Position = _positionFuture.PositionToVector();

                Vector3 src = C.Orientation * Vector3.UNIT_Z;
                Quaternion quat = src.GetRotationTo(Vector3.UNIT_Z);
                C.Rotate(quat);

                SetNextCubePosition();
                _depotCube.ReleaseMutex();
                return true;
            }
            _depotCube.ReleaseMutex();
            return false;
        }

        /// <summary>
        /// méthode de random pour savoir quel type d'ogre va naitre 1/5 pour que ca soit un batisseur
        /// </summary>
        /// <returns>le type d'ogre à créer</returns>
        public Type NaissanceOgre()
        {
            if (RND.Next(5) == 0)
            {
                //return typeof(OgreBatisseur);
                return typeof(OgreOuvrier);
            }
            else
            {
                return typeof(OgreOuvrier);
            }
        }

        private void SetNextCubePosition()
        {
            switch (_nbCubeBoisNecessaire + _nbCubePierreNecessaire)
            {
                case 22:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 21:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 20:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 19:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 18:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 17:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 16:
                    _positionFuture.ChangeValeurs(0, Cube._SIZE, 0);
                    break;
                case 15:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 14:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 13:
                    _positionFuture.ChangeValeurs(Cube._SIZE, 0, 0);
                    break;
                case 12:
                    _positionFuture.ChangeValeurs(Cube._SIZE, 0, 0);
                    break;
                case 11:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 10:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 9:
                    _positionFuture.ChangeValeurs(0, Cube._SIZE, 0);
                    break;
                case 8:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 7:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
                case 6:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 5:
                    _positionFuture.ChangeValeurs(-Cube._SIZE, 0, 0);
                    break;
                case 4:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 3:
                    _positionFuture.ChangeValeurs(0, 0, -Cube._SIZE);
                    break;
                case 2:
                    _positionFuture.ChangeValeurs(Cube._SIZE, 0, 0);
                    break;
                case 1:
                    _positionFuture.ChangeValeurs(0, 0, Cube._SIZE);
                    break;
            }
        }

    }

    /// <summary>
    /// Structure informelle pour les maisons
    /// </summary>
    public struct MaisonInfo
    {
        public String nom;
        public Vector3 position;

        public MaisonInfo(String iNom, Vector3 iPosition)
        {
            nom = iNom;
            position = iPosition;
        }
        public bool isEmpty()
        {
            if ((nom == null) && (position == Vector3.ZERO))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Reset()
        {
            nom = null;
            position = new Vector3(0, 0, 0);
        }
    }

    public struct PositionCubes
    {
        float _x;
        float _y;
        float _z;
        public PositionCubes(float x, float y, float z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }
        public void ChangeValeurs(float x, float y, float z)
        {
            this._x += x;
            this._y += y;
            this._z += z;
        }
        public Vector3 PositionToVector()
        {
            return new Vector3(_x, _y, _z);
        }
    }
}
