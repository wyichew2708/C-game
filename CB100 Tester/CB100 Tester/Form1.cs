using System.Diagnostics;
using System;
//using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.IO.Ports;

namespace CB100_Tester
{
    public partial class CB100 : Form
    {
        CommPort HeaterCOM;

        private int m_timeOutSetting = 100;
        private int m_retriesCountSetting = 0;
        private char m_STX = (char)2;
        private char m_ETX = (char)3;
        private char m_ENQ = (char)5;
        private char m_ACK = (char)6;
        private char m_NAK = (char)21;
        private char m_EOT = (char)4;
        private string m_dataReceived = "";
        private string m_dataSent = "";
        private string m_replyMessage = "";
        private string m_failMessage = "";

        private bool alarmOn = false;
        private byte[] m_ContinuousFail = new byte[4];
        private short[] m_TapeHeaterTempPrev = new short[4];

        private int m_nIdentifierInHex = 0;
        private string m_strIdentifier = "";

        public CB100()
        {
            InitializeComponent();
        }

        private void ConnectCOM_Click(object sender, EventArgs e)
        {
            HeaterCOM = new CommPort("COM" + COMTextBox.Text, 9600, "None", 8, "One");
        }

        public void ConnectHeaterCOM()
        {
            HeaterCOM = new CommPort("COM" + COMTextBox.Text, 9600, "None", 8, "One");
        }

        public void CloseHeaterCOM()
        {
            HeaterCOM.serialPort_Close();
        }

        private void ReadTemp_Click(object sender, EventArgs e)
        {
            int temp;
            //ConnectHeaterCOM();
            ReadSealHeaterTemperatureValue(1, out temp);
            //CloseHeaterCOM();
            textBox1.Text = temp.ToString();
        }

        //wychew 160330 CB100 Coding to read
        public bool ReadSealHeaterTemperatureValue(byte station, out int Temperature)
        {
            Temperature = 0;

            try
            {
                if (!HeaterCOM.serialPort1.IsOpen)
                {
                    return false;
                }
                else
                {

                    HeaterCOM.serialPort1.DiscardInBuffer();

                    //send command string
                    string first_address = (Convert.ToString(m_EOT) + "0" + station + "M1" + Convert.ToString(m_ENQ));
                    HeaterCOM.serialPort1.Write(first_address);

                    Thread.Sleep(60);       //Wait servo amplifier to response

                    string commInput = HeaterCOM.serialPort1.ReadExisting();
                 
                    if (commInput.Length > 6)
                    {
                        if (commInput.Substring(6, 1) == "") //Error
                        {
                            HeaterCOM.serialPort1.Write(Convert.ToString(m_NAK)); //Not Ack
                            Thread.Sleep(30);
                            commInput = HeaterCOM.serialPort1.ReadExisting();
                            HeaterCOM.serialPort1.Write(Convert.ToString(m_ACK)); //Ack

                            if (commInput.Length > 6)
                            {
                                if (commInput.Substring(6, 1) != "")
                                    Temperature = Convert.ToInt32(commInput.Substring(6, 3));
                            }
                        }
                        else
                        {
                            HeaterCOM.serialPort1.Write(Convert.ToString(m_ACK)); //Ack
                            Temperature = Convert.ToInt32(commInput.Substring(6, 3));
                        }
                    }
                    else
                    {
                        HeaterCOM.serialPort1.Write(Convert.ToString(m_NAK)); //Not Ack
                        Thread.Sleep(30);
                        commInput = HeaterCOM.serialPort1.ReadExisting();
                        HeaterCOM.serialPort1.Write(Convert.ToString(m_ACK)); //Ack

                        if (commInput.Length > 6)
                        {
                            if (commInput.Substring(6, 1) != "")
                                Temperature = Convert.ToInt32(commInput.Substring(6, 3));
                        }
                    }

                    HeaterCOM.serialPort1.Write(Convert.ToString(m_EOT)); //EOT

                    if (commInput == "")     //To make sure there are two byte to be compare in next function else unhandle array out of boundary will happen
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetParameters(0x01, 1);
        }

        public bool SetParameters(byte station, byte tape)
        {
            ConnectHeaterCOM();

            int SVParameter; //Set Parameter Value
            string m_Identifier;
            string ParaName;
            byte method = 0;

            while (method < 3)
            {
                switch (method)
                {
                    case 0: //Set Data Lock
                        SVParameter = 8;
                        m_Identifier = "LK";
                        ParaName = "Data Unlock";
                        break;
                    case 1: //Set SV Temp
                        SVParameter = Convert.ToInt32(SVSet.Text);

                        m_Identifier = "S1";
                        ParaName = "Max Temperature";
                        break;
                    case 2: //Set Alarm 1 Range
                        SVParameter = Convert.ToInt32(ALMSet.Text);

                        m_Identifier = "A1";
                        ParaName = "Alarm Deviation";
                        break;
                    default:  //Set Data Lock
                        SVParameter = 8;
                        m_Identifier = "LK";
                        ParaName = "Data Unlock";
                        break;
                }

                string SVParaString = SVParameter.ToString().PadLeft(3, '0');
                string m_BCC = "";
                int CurrentParameterSV = 0;
                GetBCCString(SVParaString, out m_BCC, method);

                try
                {
                    if (!HeaterCOM.serialPort1.IsOpen)
                        return false;
                    else
                    {
                        string Send_String = "";
                        string commInput = "";
                        string ReSend_String = "";

                        HeaterCOM.serialPort1.DiscardInBuffer();

                        //send command string
                        Send_String = (Convert.ToString(m_EOT) + "0" + station + Convert.ToString(m_STX) + m_Identifier + SVParaString + ".0" + Convert.ToString(m_ETX) + m_BCC);
                        ReSend_String = (Convert.ToString(m_STX) + m_Identifier + SVParaString + ".0" + Convert.ToString(m_ETX) + m_BCC);

                        HeaterCOM.serialPort1.Write(Send_String);
                        Thread.Sleep(40);
                        commInput = HeaterCOM.serialPort1.ReadExisting();


                        if (commInput == Convert.ToString((char)0x06)) //ACK
                            HeaterCOM.serialPort1.Write(Convert.ToString(m_EOT));
                        else if (commInput == Convert.ToString((char)0x15)) //NAK
                        {
                            HeaterCOM.serialPort1.Write(ReSend_String); //Set SV Value again
                            Thread.Sleep(22);
                        }
                        else
                            HeaterCOM.serialPort1.Write(Convert.ToString(m_EOT)); //EOT

                        Thread.Sleep(18); //Done Set Parameter

                        //Check Set Value
                        Send_String = (Convert.ToString(m_EOT) + "0" + station + m_Identifier + Convert.ToString(m_ENQ));

                        HeaterCOM.serialPort1.Write(Send_String);
                        Thread.Sleep(40);
                        commInput = HeaterCOM.serialPort1.ReadExisting();

                        if (commInput.Length > 6)
                        {
                            if (commInput.Substring(6, 1) == "") //Error and retry
                            {
                                HeaterCOM.serialPort1.Write(Convert.ToString(m_NAK)); //Not Ack
                                Thread.Sleep(40);
                                commInput = HeaterCOM.serialPort1.ReadExisting();
                                HeaterCOM.serialPort1.Write(Convert.ToString(m_ACK)); //Ack

                                if (commInput.Length > 6)
                                {
                                    if (commInput.Substring(6, 1) != "")
                                        CurrentParameterSV = Convert.ToInt32(commInput.Substring(6, 3));
                                }
                            }
                            else
                            {
                                HeaterCOM.serialPort1.Write(Convert.ToString(m_ACK)); //Ack
                                CurrentParameterSV = Convert.ToInt32(commInput.Substring(6, 3));
                            }
                        }
                        else //Error and retry
                        {
                            HeaterCOM.serialPort1.Write(Convert.ToString(m_NAK)); //Not Ack
                            Thread.Sleep(40);
                            commInput = HeaterCOM.serialPort1.ReadExisting();
                            HeaterCOM.serialPort1.Write(Convert.ToString(m_ACK)); //Ack

                            if (commInput.Length > 6)
                            {
                                if (commInput.Substring(6, 1) != "") //Error
                                    CurrentParameterSV = Convert.ToInt32(commInput.Substring(6, 3));
                            }
                        }

                        HeaterCOM.serialPort1.Write(Convert.ToString(m_EOT)); //EOT

                        Thread.Sleep(17); //Done Read Parameter
                    }
                }
                catch (Exception ex)
                {
                }

                method++;
            }

            CloseHeaterCOM();
            return true;
        }

        //wychew 161212 Get BCC Value
        private void GetBCCString(string SVParameter, out string m_BCC, byte type)
        {
            int DecBCC = 0;
            int m_DotHex = 0x2E;
            int m_ZeroHex = 0x30;
            int m_SVParChar1 = Convert.ToInt32(("3" + SVParameter.Substring(0, 1)), 16);
            int m_SVParChar2 = Convert.ToInt32(("3" + SVParameter.Substring(1, 1)), 16);
            int m_SVParChar3 = Convert.ToInt32(("3" + SVParameter.Substring(2, 1)), 16);

            int Identifier = 0;
            switch (type)
            {
                case 0:
                    Identifier = 0x4c ^ 0x4b;//LK
                    break;
                case 1:
                    Identifier = 0x53 ^ 0x31; //S1
                    break;
                case 2:
                    Identifier = 0x41 ^ 0x31; //A1
                    break;
            }

            //Identifier ^ Temperature ^ . ^ 0 ^ 0x03
            DecBCC = Identifier ^ m_SVParChar1 ^ m_SVParChar2 ^ m_SVParChar3 ^ m_DotHex ^ m_ZeroHex ^ 0x03;

            m_BCC = Convert.ToString((char)DecBCC);
        }
        //---

    }

    public class CommPort
    {
        #region Members Variables

        public SerialPort serialPort1;
        private string m_strDataReceived = string.Empty;
        private string m_strTempDataReceived = string.Empty;
        private string m_strPortName = string.Empty;
        //private char m_cSTX = (char)2;
        private char m_cETX = (char)3;
        //private char m_cEOT = (char)4;
        //private char m_cENQ = (char)5;
        private char m_cACK = (char)6;
        private char m_cENTER = (char)13;
        private char m_cNAK = (char)21;

        private int m_nCOMStage = 0;
        private bool m_bETXCOM = false;
        private bool m_bACKCOM = false;

        #endregion


        public CommPort(string portName, int baudRate, string noParity, int dataBits, string stopBits)
        {
            serialPort1 = new System.IO.Ports.SerialPort();
            m_strPortName = portName;
            InitComPort(baudRate, noParity, dataBits, stopBits);
        }

        public CommPort(string portName)
        {
            serialPort1 = new System.IO.Ports.SerialPort();
            m_strPortName = portName;
            InitComPort();
        }

        public bool InitComPort(int baudRate, string noParity, int dataBits, string stopBits)
        {
            try
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();

                serialPort1.PortName = m_strPortName;
                serialPort1.BaudRate = baudRate;
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), noParity);          // (Parity)Enum.Parse(typeof(Parity), "None");
                serialPort1.DataBits = dataBits;
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits);    //(StopBits)Enum.Parse(typeof(StopBits), "One");
                serialPort1.Handshake = Handshake.None;                                     // (Handshake)Enum.Parse(typeof(Handshake), "None");
                serialPort1.DtrEnable = true;
                //serialPort1.DiscardNull = true;                                             //null will be discard
                //serialPort1.ReceivedBytesThreshold = 1;
                //serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);

                //Modbus COM
                serialPort1.RtsEnable = true;

                serialPort1.Open();
                return true;
            }
            catch
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();

                MessageBox.Show("Open " + m_strPortName + " fail. " + m_strPortName + " is using by another program.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool InitComPort()
        {
            try
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();

                serialPort1.PortName = m_strPortName;
                serialPort1.BaudRate = int.Parse("9600");
                serialPort1.Parity = Parity.None;           // (Parity)Enum.Parse(typeof(Parity), "None");
                serialPort1.DataBits = int.Parse("8");
                serialPort1.StopBits = StopBits.One;        //(StopBits)Enum.Parse(typeof(StopBits), "One");
                serialPort1.Handshake = Handshake.None;     // (Handshake)Enum.Parse(typeof(Handshake), "None");
                serialPort1.DtrEnable = true;
                serialPort1.DiscardNull = true;             //null will be discard
                serialPort1.ReceivedBytesThreshold = 1;
                serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);

                serialPort1.Open();
                return true;
            }
            catch
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();

                MessageBox.Show("Open " + m_strPortName + " fail. " + m_strPortName + " is using by another program.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static byte[] StrToByteArray(string Str)
        {
            // C# to convert a string to a byte array.
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(Str);
        }

        public static string ByteArrayToStr(byte[] Byte)
        {
            // C# to convert a byte array to a string.
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetString(Byte);
        }

        public void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] inByte = new byte[serialPort1.BytesToRead];
            serialPort1.Read(inByte, 0, inByte.Length);
            string inData = ByteArrayToStr(inByte);

            if (inData != "")
            {
                switch (m_nCOMStage)
                {
                    case 1:
                        //Pooling
                        m_strTempDataReceived += inData;
                        for (int i = 0; i < inData.Length; i++)
                        {
                            if (inData[i].ToString() == m_cETX.ToString())
                            {
                                m_bETXCOM = true;
                                break;
                            }
                        }

                        if (m_bETXCOM)
                        {
                            m_nCOMStage = 0;
                            m_strDataReceived = m_strTempDataReceived.Trim(m_cETX);
                            m_strTempDataReceived = string.Empty;
                        }
                        break;

                    case 2:
                        //Selecting
                        if (inData[inData.Length - 1].ToString() == m_cACK.ToString())
                        {
                            m_bACKCOM = true;
                            m_nCOMStage = 0;
                        }
                        else if (inData[inData.Length - 1].ToString() == m_cNAK.ToString())
                        {
                            m_bACKCOM = false;
                            m_nCOMStage = 0;
                        }
                        break;

                    case 3:
                        //Get Enter
                        m_strTempDataReceived += inData;

                        if (inData[inData.Length - 1].ToString() == m_cENTER.ToString())
                        {
                            m_nCOMStage = 0;
                            m_strDataReceived = m_strTempDataReceived.Trim(m_cENTER);
                            m_strTempDataReceived = string.Empty;
                            break;
                        }
                        break;

                    case 4:
                        //Pooling Model Code
                        m_strTempDataReceived += inData;
                        break;

                    case 5:
                        //Normal Serial Port Data Received Function
                        m_strDataReceived = inData;
                        break;

                    default:
                        break;
                }
            }
        }

        public void serialPort_WriteData(string command)
        {
            serialPort1.Write(command);
        }

        public string serialPort_GetData()
        {
            return m_strDataReceived;
        }

        public bool serialPort_GetACKCOMStatus()
        {
            return m_bACKCOM;
        }

        public bool serialPort_GetETXCOMStatus()
        {
            return m_bETXCOM;
        }

        public void serialPort_SetCOMStageStatus(int ComStage)
        {
            m_nCOMStage = ComStage;
        }

        public void serialPort_ClearData()
        {
            m_strDataReceived = string.Empty;
        }

        public void serialPort_Close()
        {
            try
            {
                serialPort_ClearData();
                m_strPortName = string.Empty;
                serialPort1.Close();
            }
            catch
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();

                MessageBox.Show("Close " + m_strPortName + " fail. " + m_strPortName + " is using by another program.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool serialPort_IsOpen()
        {
            if (serialPort1.IsOpen)
                return true;
            else
                return false;
        }
    }
}