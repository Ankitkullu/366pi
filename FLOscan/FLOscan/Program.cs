using FloSDK.Methods;
using FloSDK.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Newtonsoft.Json.Linq;

namespace FLOscan
{
     class Program
    {
       static string username = ConfigurationManager.AppSettings.Get("username");
       static string password = ConfigurationManager.AppSettings.Get("password");
       static string wallet_url = ConfigurationManager.AppSettings.Get("wallet_url");
        static string wallet_port = ConfigurationManager.AppSettings.Get("wallet_port");
        static void Main(string[] args)
        {
            RpcMethods rpc = new RpcMethods( username,password,wallet_url,wallet_port);
            int startblock = 0;
            int blockcount =0;
            int searchcount = 0;
            string searchpattern = "";

            Console.WriteLine("FLO blockchain scan utility ");
            Console.WriteLine("----------------------------");

            Console.WriteLine("Enter the Block no. from where to start the search:");
            startblock= Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("enter the sreach pattern");
            searchpattern= Console.ReadLine();

            Console.WriteLine("**********************");
            Console.WriteLine("Search Started:");
            Console.WriteLine("**********************");


            try
            {
                // get block count
                JObject obj = JObject.Parse(rpc.GetBlockCount());
                if (string.IsNullOrEmpty(obj["error"].ToString()))
                {
                    blockcount = Convert.ToInt32(obj["result"].ToString());
                    Console.WriteLine("No. of blocks : " +blockcount);
                }
                else
                {
                    Console.WriteLine("Error getting blockcount : " + obj["error"]);
                }
                //iterate through all blocks
                for(int i=startblock;i<=blockcount;i++)
                {
                        try
                    {
                        string blockhash = "";
                         obj = JObject.Parse(rpc.GetBlockHash(i));

                        if (string.IsNullOrEmpty(obj["error"].ToString()))
                        {
                            blockhash = obj["result"].ToString();

                            JObject obj1=JObject.Parse(rpc.GetBlock(blockhash));
                            if (string.IsNullOrEmpty(obj1["error"].ToString()))
                            {
                                JArray txs = JArray.Parse(obj1["result"]["tx"].ToString());
                                //iterating all transaction
                                foreach(JValue tx in txs)
                                {
                                    JObject obj2= JObject.Parse(rpc.GetRawTransaction(tx.ToString()));
                                    string flodata = "";

                                    if (string.IsNullOrEmpty(obj["error"].ToString()))
                                    {
                                        flodata= obj2["result"]["floData"].ToString();

                                        if(flodata.Contains(searchpattern)) 
                                        {
                                            searchcount++;
                                            Console.WriteLine("----------");
                                            Console.WriteLine("found at block No :" + i);
                                            Console.WriteLine("FLodata :"+flodata);
                                            Console.WriteLine("-------------");


                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error getting blockcount : " + obj1["error"]);
                                    } 

                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error getting blockchain for block no. : " + obj["error"]);
                        }


                    }
                    catch(RpcInternalServerErrorException e) 
                    {
                        continue;
                    }
                    catch(Exception e1) 
                    {
                        continue;
                    }
                }

            }
            catch (RpcInternalServerErrorException ex)
            {
                var errorcode = 0;
                var errormessage=string.Empty;

                if (ex.RpcErrorCode.GetHashCode() != 0)
                {
                    errorcode = ex.RpcErrorCode.GetHashCode();
                    errormessage = ex.RpcErrorCode.ToString();
                }
                Console.WriteLine("Exception : " + errormessage + " --" + errormessage);
               
            }
            catch (Exception ex1)
            {
                Console.WriteLine("Exception : " + ex1.ToString());
                
            }

            Console.WriteLine("Search is complete and found" + searchcount + "times");
            Console.WriteLine("no. Blocks searched :" + (blockcount - startblock));
            Console.Read();
        }


        }
    }
 
