using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeInject
{


    public unsafe partial class MainMenu : Form
    {
        #region externals
        [DllImport("ClrBootstrap.dll")]
        public static extern UInt64 GetBaseAdress();

        [DllImport("ClrBootstrap.dll")]
        public static extern UInt64 GetInt64(UInt64 Adress);
        [DllImport("ClrBootstrap.dll")]
        public static extern int GetInt32(UInt64 Adress);
        [DllImport("ClrBootstrap.dll")]
        public static extern float GetFloat(UInt64 Adress);
        [DllImport("ClrBootstrap.dll")]
        public static extern int SendPacketToServer(UInt64 deviceAddr, byte[] packet);

        [DllImport("ClrBootstrap.dll")]
        public static extern byte GetByte(UInt64 Adress);


        [DllImport("ClrBootstrap.dll")]
        public static extern short GetShort(UInt64 Adress);

        [DllImport("ClrBootstrap.dll")]
        public static extern void GetByteArray(UInt64 adress, byte[] outTable, int size);
        #endregion

        public class Actor
        {
            public int Id; // 0x74
            public string Name;
            public int Addres;
            public float* PosX;
            public float* PosY;
            public float* PosZ;

            public override string ToString()
            {
                return Name;
            }
        }

        public List<Actor> ActorList { get; set; } = new List<Actor>();


        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ActiveObject(void* thisArg0, void* ObjectAddr);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        unsafe delegate void CallAction(void* argThis, int arg0, int actionID, uint targetID, int arg3, int arg4, int arg5, int arg6, int arg7);

        /// <summary>
        /// MonsterAdr() + 0x74 targetID address
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="targetID"></param>
        public void ActionCommand(int actionId, uint targetID = 0xE0000000)
        {
            CallAction delegateRecive = (CallAction)Marshal.GetDelegateForFunctionPointer(new IntPtr((long)(GetBaseAdress() + 0x8055C0)), typeof(CallAction));
            delegateRecive((new IntPtr((long)(GetBaseAdress() + 0x16C3DD0)).ToPointer()), 1, actionId, targetID,0, 0, 0, 0, 0);
        }

        int* objectCount;


        public MainMenu()
        {
            InitializeComponent();


            objectCount = (int*)(new IntPtr(GetInt32((GetBaseAdress() + 0x180D1B4))+0xEB4).ToPointer());
            MessageBox.Show((*objectCount).ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(lNearObjects.SelectedIndex!=-1)
            {
                label1.Text = $"POS: x:{*((Actor)lNearObjects.SelectedItem).PosX} y:{*((Actor)lNearObjects.SelectedItem).PosY} z:{*((Actor)lNearObjects.SelectedItem).PosZ}";
            }
        }


        public int MonsterAdr(int index)
        {
            int* firstObjectsElement = objectCount;
            firstObjectsElement++;

            firstObjectsElement+= index;

            return *firstObjectsElement;
        }


        public void resetList()
        {
            ActorList.Clear();

            int* firstObjectsElement = objectCount;
            lObjectCount.Text = $"Count: {*objectCount}";
            firstObjectsElement++;

            for (int i = 0; i < *objectCount; i++)
            {
                Actor actor = new Actor
                {
                    Name = Marshal.PtrToStringAnsi(new IntPtr(*firstObjectsElement + 0x30)),
                    Addres = (*firstObjectsElement),
                    Id = (*(firstObjectsElement + 0x74)),
                    PosX = (float*)new IntPtr(*firstObjectsElement + 0x30 + 0x80).ToPointer(),
                    PosZ = (float*)new IntPtr(*firstObjectsElement + 0x30 + 0x84).ToPointer(),
                    PosY = (float*)new IntPtr(*firstObjectsElement + 0x30 + 0x88).ToPointer()
                };
                ActorList.Add(actor);
                firstObjectsElement++;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            resetList();
            ActiveObject activeObject = (ActiveObject)Marshal.GetDelegateForFunctionPointer(new IntPtr((long)(GetBaseAdress() + 0x425a70)), typeof(ActiveObject));
            int* thisADDR = (int*)(GetBaseAdress() + 0x180D1B4);
            if (lNearObjects.SelectedIndex != -1 && *thisADDR != 0x0)
            {
                int selectObjectIndex = ActorList.FindIndex(x => ((Actor)lNearObjects.SelectedItem).Addres == x.Addres);
                if (selectObjectIndex != -1)
                {
                    Actor player = ActorList[0];
                    *player.PosX = *((Actor)lNearObjects.SelectedItem).PosX;
                    *player.PosY = *((Actor)lNearObjects.SelectedItem).PosY;
                    *player.PosZ = *((Actor)lNearObjects.SelectedItem).PosZ;
                    activeObject(new IntPtr(*thisADDR).ToPointer(), new IntPtr(MonsterAdr(selectObjectIndex)).ToPointer());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lNearObjects.Items.Clear();
            ActorList.Clear();

            int* firstObjectsElement = objectCount;
            lObjectCount.Text = $"Count: {*objectCount}";
            firstObjectsElement++;

            for (int i = 0; i < *objectCount; i++)
            {
                Actor actor = new Actor
                {
                    Name = Marshal.PtrToStringAnsi(new IntPtr(*firstObjectsElement + 0x30)),
                    Addres = (*firstObjectsElement),
                    Id = (*(firstObjectsElement + 0x74)),
                    PosX = (float*)new IntPtr(*firstObjectsElement + 0x30 + 0x80).ToPointer(),
                    PosZ = (float*)new IntPtr(*firstObjectsElement + 0x30 + 0x84).ToPointer(),
                    PosY = (float*)new IntPtr(*firstObjectsElement + 0x30 + 0x88).ToPointer()
                };
                ActorList.Add(actor);
                lNearObjects.Items.Add(actor);
                firstObjectsElement++;
            }
        }

        private void lNearObjects_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
