using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace csutils
{
    public class SysInfo
    {
        public static IEnumerable<string> getSomeInfo()
        {
            var list = from p in Process.GetProcesses()
                       select p.ProcessName;

            return list;
        }

        #region 将字符串形式的IP地址转换成IPAddress对象
        /// <summary>
        /// 将字符串形式的IP地址转换成IPAddress对象
        /// </summary>
        /// <param name="ip">字符串形式的IP地址</param>        
        public static IPAddress StringToIPAddress(string ip)
        {
            return IPAddress.Parse(ip);
        }
        #endregion

        #region 获取本机的计算机名
        /// <summary>
        /// 获取本机的计算机名
        /// </summary>
        public static string LocalHostName
        {
            get
            {
                return Dns.GetHostName();
            }
        }
        #endregion

        #region 获取本机的局域网IP
        /// <summary>
        /// 获取本机的局域网IP
        /// </summary>        
        public static string LANIP
        {
            get
            {
                //获取本机的IP列表,IP列表中的第一项是局域网IP，第二项是广域网IP
                IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

                //如果本机IP列表为空，则返回空字符串
                if (addressList.Length < 1)
                {
                    return "";
                }

                //返回本机的局域网IP
                return addressList[0].ToString();
            }
        }
        #endregion

        #region 获取所有本机ip
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetAllIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostEntry(hostName);    //方法已过期，可以获取IPv4的地址
                                                                   //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
                                                                   //IPAddress localaddr = localhost.AddressList[0];
            List<IPAddress> list = new List<IPAddress>();
            foreach (var item in localhost.AddressList)
            {
                if (item.AddressFamily==System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    list.Add(item);
                } 
            }
            return DataUtil.ListToString(list);
        }
        #endregion

        #region 获取局域网IP
        public static string getLocalIpAddress()
        {
            DisplayIPv4NetworkInterfaces();
            //IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                
                if (adapter.OperationalStatus==OperationalStatus.Down|| adapter.NetworkInterfaceType==NetworkInterfaceType.Loopback)
                {
                    continue;
                }
                Console.WriteLine("Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                //Console.WriteLine("OperationalStatus.......{0}", adapter.OperationalStatus);
                IPInterfaceProperties properties = adapter.GetIPProperties();

                //adapter.GetIPv4Statistics();
                UnicastIPAddressInformationCollection uniCast = properties.UnicastAddresses;
                if (uniCast != null)
                {
                    foreach (UnicastIPAddressInformation uni in uniCast)
                    {
                        if (uni.PrefixOrigin==PrefixOrigin.Dhcp)
                        {
                            return uni.Address.ToString();


                        }
                        Console.WriteLine("{0}.......{1}", uni.Address,
                            uni.IPv4Mask
                            //uni.DuplicateAddressDetectionState
                            );
                        //Console.WriteLine("     Prefix Origin ........................ : {0}", uni.PrefixOrigin);
                        //Console.WriteLine("     Suffix Origin ........................ : {0}", uni.SuffixOrigin);
                        //Console.WriteLine("     Duplicate Address Detection .......... : {0}",
                        //    uni.DuplicateAddressDetectionState);
                    }
                    Console.WriteLine();
                }

            }
            return "0.0.0.0";
        }
        public static string getLocalIPv4Address()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                // Only display informatin for interfaces that support IPv4.
                if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                {
                    continue;
                }
                Console.WriteLine(adapter.Description);
                // Underline the description.
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                // Try to get the IPv4 interface properties.
                IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();
                Console.WriteLine();
            }
            return "";
        }
        #endregion

        #region
        public static void ShowNetworkInterfaces()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                    computerProperties.HostName, computerProperties.DomainName);
            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return;
            }

            Console.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Console.WriteLine();
                Console.WriteLine(adapter.Description);
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                Console.WriteLine("  Physical Address ........................ : {0}",
                           adapter.GetPhysicalAddress().ToString());
                Console.WriteLine("  Operational status ...................... : {0}",
                    adapter.OperationalStatus);
                string versions = "";

                // Create a display string for the supported IP versions.
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    versions = "IPv4";
                }
                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                    {
                        versions += " ";
                    }
                    versions += "IPv6";
                }
                Console.WriteLine("  IP version .............................. : {0}", versions);
                ShowIPAddresses(properties);
                Console.WriteLine("{0}", properties.GetIPv4Properties().ToString());

                // The following information is not useful for loopback adapters.
                //if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                //{
                //    continue;
                //}
                //Console.WriteLine("  DNS suffix .............................. : {0}",
                //    properties.DnsSuffix);

                //string label;
                //if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                //{
                //    IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                //    Console.WriteLine("  MTU...................................... : {0}", ipv4.Mtu);
                //    if (ipv4.UsesWins)
                //    {

                //        IPAddressCollection winsServers = properties.WinsServersAddresses;
                //        if (winsServers.Count > 0)
                //        {
                //            label = "  WINS Servers ............................ :";
                //            //ShowIPAddresses(label, winsServers);
                //        }
                //    }
                //}

                //Console.WriteLine("  DNS enabled ............................. : {0}",
                //    properties.IsDnsEnabled);
                //Console.WriteLine("  Dynamically configured DNS .............. : {0}",
                //    properties.IsDynamicDnsEnabled);
                //Console.WriteLine("  Receive Only ............................ : {0}",
                //    adapter.IsReceiveOnly);
                //Console.WriteLine("  Multicast ............................... : {0}",
                //    adapter.SupportsMulticast);

                Console.WriteLine();
            }
        }
        public static void ShowIPAddresses(IPInterfaceProperties adapterProperties)
        {
            //IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
            //if (dnsServers != null)
            //{
            //    foreach (IPAddress dns in dnsServers)
            //    {
            //        Console.WriteLine("  DNS Servers ............................. : {0}",
            //            dns.ToString()
            //       );
            //    }
            //}
            //IPAddressInformationCollection anyCast = adapterProperties.AnycastAddresses;
            //if (anyCast != null)
            //{
            //    foreach (IPAddressInformation any in anyCast)
            //    {
            //        Console.WriteLine("  Anycast Address .......................... : {0} {1} {2}",
            //            any.Address,
            //            any.IsTransient ? "Transient" : "",
            //            any.IsDnsEligible ? "DNS Eligible" : ""
            //        );
            //    }
            //    Console.WriteLine();
            //}

            //MulticastIPAddressInformationCollection multiCast = adapterProperties.MulticastAddresses;
            //if (multiCast != null)
            //{
            //    foreach (IPAddressInformation multi in multiCast)
            //    {
            //        Console.WriteLine("  Multicast Address ....................... : {0} {1} {2}",
            //            multi.Address,
            //            multi.IsTransient ? "Transient" : "",
            //            multi.IsDnsEligible ? "DNS Eligible" : ""
            //        );
            //    }
            //    Console.WriteLine();
            //}
            UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
            if (uniCast != null)
            {
                string lifeTimeFormat = "dddd, MMMM dd, yyyy  hh:mm:ss tt";
                foreach (UnicastIPAddressInformation uni in uniCast)
                {
                    DateTime when;

                    Console.WriteLine("  Unicast Address ......................... : {0}", uni.Address);
                    Console.WriteLine("     Prefix Origin ........................ : {0}", uni.PrefixOrigin);
                    Console.WriteLine("     Suffix Origin ........................ : {0}", uni.SuffixOrigin);
                    Console.WriteLine("     Duplicate Address Detection .......... : {0}",
                        uni.DuplicateAddressDetectionState);

                    // Format the lifetimes as Sunday, February 16, 2003 11:33:44 PM
                    // if en-us is the current culture.

                    // Calculate the date and time at the end of the lifetimes.    
                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.AddressValidLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     Valid Life Time ...................... : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                    );
                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.AddressPreferredLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     Preferred life time .................. : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                    );

                    when = DateTime.UtcNow + TimeSpan.FromSeconds(uni.DhcpLeaseLifetime);
                    when = when.ToLocalTime();
                    Console.WriteLine("     DHCP Leased Life Time ................ : {0}",
                        when.ToString(lifeTimeFormat, System.Globalization.CultureInfo.CurrentCulture)
                    );
                }
                Console.WriteLine();
            }
        }
        public static void DisplayIPv4NetworkInterfaces()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            Console.WriteLine("IPv4 interface information for {0}.{1}",
               properties.HostName, properties.DomainName);
            Console.WriteLine();

            foreach (NetworkInterface adapter in nics)
            {
                // Only display informatin for interfaces that support IPv4.
                if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                {
                    continue;
                }
                Console.WriteLine(adapter.Description);
                // Underline the description.
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                // Try to get the IPv4 interface properties.
                IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();

                if (p == null)
                {
                    Console.WriteLine("No IPv4 information is available for this interface.");
                    Console.WriteLine();
                    continue;
                }
                // Display the IPv4 specific data.
                Console.WriteLine("  Index ............................. : {0}", p.Index);
                Console.WriteLine("  MTU ............................... : {0}", p.Mtu);
                Console.WriteLine("  APIPA active....................... : {0}",
                    p.IsAutomaticPrivateAddressingActive);
                Console.WriteLine("  APIPA enabled...................... : {0}",
                    p.IsAutomaticPrivateAddressingEnabled);
                Console.WriteLine("  Forwarding enabled................. : {0}",
                    p.IsForwardingEnabled);
                Console.WriteLine("  Uses WINS ......................... : {0}",
                    p.UsesWins);
                Console.WriteLine();
            }
        }
        #endregion
    }
}
