using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeInject
{
    static class GameHackMethods
    {
        /// <summary>
        /// Monster atttack function 
        /// </summary>
        /// <param name="skillId">Skill Id</param>
        /// <param name="monsterIndex"></param>
        /// <returns></returns>
        [DllImport("ClrBootstrap.dll")]
        public static extern int AttackTarget(uint skillId, uint monsterIndex);
        [DllImport("ClrBootstrap.dll")]
        public static extern ulong  GetBaseAdress();
    }
}
