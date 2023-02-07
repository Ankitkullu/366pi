using FloSDK.Exceptions;
using FloSDK.Methods;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FLOtesting
{
    public partial class Form1 : Form
    {
        public static string receiverAddress = "ofewGM44vtMYfszdPQ99tQKDynmvjoAWJW";
        public string url = "https://testnet.flocha.in/tx/";
        public Form1()
        {
            InitializeComponent();
        }

        private void send_Click(object sender, EventArgs e)
        {
            string username = ConfigurationManager.AppSettings.Get("username");
            string password = ConfigurationManager.AppSettings.Get("password");
            string wallet_url = ConfigurationManager.AppSettings.Get("wallet_url");
            string wallet_port = ConfigurationManager.AppSettings.Get("wallet_port");

            RpcMethods obj = new RpcMethods(username, password, wallet_url, wallet_port);
            try
            {
                string Sender = senderTxT.Text;
                string SenderAddress = senderAddress.Text;
                string Receiver = receiver.Text;
                string ReceiverAddress = receiver.Text;
                string Amount = amount.Text;
                string Message = message.Text;
                string flodata = "recevied the amount from " + Sender + " of " + SenderAddress + " for an amount of " + Amount + "FLo to" + Receiver + " of " + ReceiverAddress + " # " + Message;

                JObject jobj = JObject.Parse(obj.SendToAddress(receiverAddress,0.1M,"366pi","366pi",false,false,1,"UNSET",flodata));
                if (string.IsNullOrEmpty(jobj["error"].ToString()))
                    {
                    url += jobj["result"];
                    lblmessage.Text = "Transaction is successfull";
                    lblmessage.ForeColor = Color.Blue;
                    lblmessage.Visible =true;
                    linkLabel1.Visible =true;
                    }
                else
                {
                    lblmessage.Text = "error in transaction ";
                    lblmessage.ForeColor = Color.Red;
                    lblmessage.Visible = true;
                    linkLabel1.Visible = false;
                }
            }
            catch (RpcInternalServerErrorException ex)
            {
                var errcode = 0;
                var errMessage = string.Empty;

                if (ex.RpcErrorCode.GetHashCode() != 0) 
                {
                    errcode = ex.RpcErrorCode.GetHashCode();
                    errMessage = ex.RpcErrorCode.ToString();
                }

                Console.WriteLine("Exception : " + errcode + " - " + errMessage);
                lblmessage.Text = "error in transaction ";
                lblmessage.ForeColor = Color.Red;
                lblmessage.Visible = true;
                linkLabel1.Visible = false;
            }
            catch(Exception ex1) 
            {
                Console.WriteLine("Exception : " + ex1.ToString());
                lblmessage.Text = "error in transaction ";
                lblmessage.ForeColor = Color.Red;
                lblmessage.Visible = true;
                linkLabel1.Visible = false; 
            }

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(url); 
        }
    }
}
    