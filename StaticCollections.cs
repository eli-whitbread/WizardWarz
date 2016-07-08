using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardWarz
{
    /// <summary>
    /// Globally accessible collections should be placed in this class
    /// </summary>
    class StaticCollections
    {
        public Int32 maxBombsInLevel = 10;
        protected static Bomb[] levelBombInstances;

        public StaticCollections()
        {
            levelBombInstances = new Bomb[maxBombsInLevel];
        }

        public static void SendBombUpdate()
        {
            for (int i = 0; i < levelBombInstances.Length; i++)
            {
                if (levelBombInstances[i] != null)
                {
                    levelBombInstances[i].BombTickUpdate();
                }
            }
        }

        //add a bomb to the collection
        public static void AddBomb(Bomb bombToAdd)
        {
            for (int i = 0; i < levelBombInstances.Length; i++)
            {
                if (levelBombInstances[i] == null)
                {
                    levelBombInstances[i] = bombToAdd;
                }
            }
        }

        //remove a bomb from the collection
        public static void RemoveBomb(Bomb bombToRemove)
        {
            for (int i = 0; i < levelBombInstances.Length; i++)
            {
                if (levelBombInstances[i] == bombToRemove)
                {
                    levelBombInstances[i] = null;
                }
            }
        }



    }
}
