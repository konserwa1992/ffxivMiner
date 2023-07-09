using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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



        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        public delegate void ActiveObject(void* thisArg0, void* ObjectAddr);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        unsafe delegate void CallAction(void* argThis, int arg0, int actionID, uint targetID, int arg3, int arg4, int arg5, int arg6, int arg7);



        /// <summary>
        /// MonsterAdr() + 0x74 to adress targetu 
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="targetID"></param>
        public void ActionCommand(int actionId, uint targetID = 0xE0000000)
        {
            CallAction delegateRecive = (CallAction)Marshal.GetDelegateForFunctionPointer(new IntPtr((long)(GetBaseAdress() + 0x8055C0)), typeof(CallAction));
            delegateRecive((new IntPtr((long)(GetBaseAdress() + 0x16C3DD0)).ToPointer()), 1, actionId, targetID,0, 0, 0, 0, 0);
        }

        /// <summary>
        /// 00001D6D
        /// </summary>

        int* objectCount;


        public MainMenu()
        {
            InitializeComponent();


            objectCount = (int*)(new IntPtr((long)(GetBaseAdress() + 0x18051D4)).ToPointer());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lNearObjects.Items.Clear();

            int* firstObjectsElement = objectCount;
            lObjectCount.Text = $"Count: {*objectCount}";
            firstObjectsElement++;

            for (int i = 0;i<*objectCount; i++)
            {
                string str = Marshal.PtrToStringAnsi(new IntPtr(*firstObjectsElement+0x30));
                lNearObjects.Items.Add((str));
                firstObjectsElement++;
            }
        }


        public int MonsterAdr(int index)
        {
            int* firstObjectsElement = objectCount;
            firstObjectsElement++;

            firstObjectsElement+= index;

            return *firstObjectsElement;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(sizeof(int).ToString("X"));
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
          //  MessageBox.Show(Marshal.PtrToStringAnsi(new IntPtr(MonsterAdr(lNearObjects.SelectedIndex) + 0x30)));
            ActiveObject delegateRecive = (ActiveObject)Marshal.GetDelegateForFunctionPointer(new IntPtr((long)(GetBaseAdress() + 0x425A70)), typeof(ActiveObject));
            int* thisADDR = (int*)(GetBaseAdress() + 0x1804134);
            if (lNearObjects.SelectedIndex != -1)
                delegateRecive(new IntPtr(*thisADDR).ToPointer(), new IntPtr(MonsterAdr(lNearObjects.SelectedIndex)).ToPointer());

        }

        private void button2_Click(object sender, EventArgs e)
        {

            lNearObjects.Items.Clear();

            int* firstObjectsElement = objectCount;
            lObjectCount.Text = $"Count: {*objectCount}";
            firstObjectsElement++;

            for (int i = 0; i < *objectCount; i++)
            {
                string str = Marshal.PtrToStringAnsi(new IntPtr(*firstObjectsElement + 0x30));
                lNearObjects.Items.Add((str));
                firstObjectsElement++;
            }
        }


    }
}
