using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Three_Layers;

namespace Three_Layers.Core
{
    class UserBLL
    {
        private UserDAL userDAL;
  
        /// <summary>
        /// Свойства business logic layer.
        /// </summary>
        public string FamiliaFind { set; get; }
        public string NameFind { set; get; }
        public string OtchestvoFind { set; get; }
        public string BankNameFind { set; get; }

        public List<Client> clients;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UserBLL()
        {
            userDAL = new UserDAL();
            clients = userDAL.GetInfoFromFile();
        }

        /// <summary>
        /// Метод сортировки и отправки коллекции клиентов на запись в файл.
        /// </summary>
        /// <param name="clientList"></param>
        public void NewClient(List<Client> clientList)
        {
            if (clientList.Count > 1)
            {
                clientList.Sort(delegate(Client x, Client y)
                {
                    return x.BankName.CompareTo(y.BankName);
                });
            }
            userDAL.PutInfoToFile(clientList);
        }

        // Применение фильтров.
        public string FilterClients(string familiaFind, string nameFind, string otchestvoFind, string bankNameFind)
        {
            List<Client> clientList = clients;

            try
            {
                // 1) По фамилии.
                var queryFamilia =
                            from client in clientList
                            where client.Fio.ToLower().Contains(familiaFind.ToLower())
                            select new
                            {
                                Fio = client.Fio,
                                Birthday = client.Birthday,
                                BankName = client.BankName
                            };

                // 2) По имени.
                var queryName =
                            from client in queryFamilia
                            where client.Fio.ToLower().Contains(nameFind.ToLower())
                            select new
                            {
                                Fio = client.Fio,
                                Birthday = client.Birthday,
                                BankName = client.BankName
                            };

                // 3) По отчеству.
                var queryOtchestvo =
                            from client in queryName
                            where client.Fio.ToLower().Contains(otchestvoFind.ToLower())
                            select new
                            {
                                Fio = client.Fio,
                                Birthday = client.Birthday,
                                BankName = client.BankName
                            };

                // 4) По банку.
                var queryBankName =
                            from client in queryOtchestvo
                            where client.BankName.ToLower().Contains(bankNameFind.ToLower())
                            select new
                            {
                                Fio = client.Fio,
                                Birthday = client.Birthday,
                                BankName = client.BankName
                            };

                string answer = "";

                foreach (var client in queryBankName)
                {
                    answer += client.Fio + " - " + String.Format("{0}.{1}.{2}", client.Birthday.Day, client.Birthday.Month, client.Birthday.Year) + " - " + client.BankName + Environment.NewLine;
                }

                List<Client> query = new List<Client>();
                foreach (var item in queryBankName)
                {
                    query.Add(new Client() { Fio = item.Fio, Birthday = item.Birthday, BankName = item.BankName });
                }

                if(userDAL.SerializeResult(query))
                {
                    answer += Environment.NewLine + "Результат сереализован. Файл: Result.xml";
                }

                return answer;
            }
            catch
            {
                Console.WriteLine("Ошибка фильтра.");
                return null;
            }
        }

    }
}
