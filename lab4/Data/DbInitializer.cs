using System.Threading.Tasks;

namespace lab4.Data
{
    public static class DbInitializer
    {

        private static Random random = new Random();
        private static List<string> Additionalconditions1 = new()
            {
                "Ежемесячная капитализация процентов",
                "Гарантированная минимальная ставка",
                "Возможность досрочного закрытия вклада без штрафов",
                "Специальные условия для пенсионеров",
                "Бесплатная карта для владельцев вкладов",
                "Онлайн-мониторинг и управление вкладом",
                "Приветственный бонус для новых клиентов",
                "Специальные условия для долгосрочных вкладов",
                "Страхование вклада от непредвиденных ситуаций",
                "Бонусы и скидки от партнеров банка",
                "Возможность автоматического продления вклада",
                "Гибкая система начисления процентов",
                "Бесплатные консультации финансового консультанта",
                "Дополнительный бонус при открытии нескольких вкладов",
                "Уникальные привилегии для VIP-клиентов",
                "Специальные условия для студентов и молодежи",
                "Гарантированный возврат основной суммы вклада",
                "Бесплатный доступ к онлайн-курсам по финансам",
                "Бонус за сохранение вклада на определенный срок",
                "Персональный менеджер для обслуживания вклада"
            };

        private static List<string> Name1 = new()
            {
                "Анна",
                "Владимир",
                "Ольга",
                "Артем",
                "Наталья",
                "Сергей",
                "Татьяна",
                "Михаил",
                "Елена",
                "Денис",
                "Екатерина",
                "Игорь",
                "Юлия",
                "Дмитрий",
                "Алина",
                "Ирина",
                "Павел",
                "Марина",
                "Александр",
                "Евгения"
            };

        private static List<string> Surname1 = new()
            {
                "Иванов",
                "Петров",
                "Смирнов",
                "Кузнецов",
                "Васильев",
                "Соколов",
                "Михайлов",
                "Новиков",
                "Федоров",
                "Морозов",
                "Волков",
                "Алексеев",
                "Лебедев",
                "Семенов",
                "Егоров",
                "Павлов",
                "Козлов",
                "Степанов",
                "Николаев",
                "Орлов"
            };

        private static List<string> Middlename1 = new()
            {
                "Иванович",
                "Петрович",
                "Сергеевич",
                "Александрович",
                "Дмитриевич",
                "Николаевич",
                "Андреевич",
                "Михайлович",
                "Артемович",
                "Владимирович",
                "Егорович",
                "Алексеевич",
                "Васильевич",
                "Павлович",
                "Аркадьевич",
                "Федорович",
                "Анатольевич",
                "Игоревич",
                "Станиславович",
                "Олегович"
            };

        public static void Initialize(BankDepositsContext db)
        {
            db.Database.EnsureCreated();

            if (!db.Emploees.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    string Name = Name1[random.Next(0, Name1.Count)];
                    string Surname = Surname1[random.Next(0, Surname1.Count)];
                    string Middlename = Middlename1[random.Next(0, Middlename1.Count)];
                    string Post = "Банкир " + i;
                    DateTime Dob = new DateTime(random.Next(1980, 2001), random.Next(1, 13), random.Next(1, 29));
                    db.Add(new Emploee() { Name = Name, Surname = Surname, Middlename = Middlename, Post = Post, Dob = Dob });
                }
                db.SaveChanges();
            }
                        
            if (!db.Investors.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    string Name = Name1[random.Next(0,  Name1.Count)];
                    string Surname = Surname1[random.Next(0, Surname1.Count)];
                    string Middlename = Middlename1[random.Next(0, Middlename1.Count)];
                    string Address = "Адрес " + i;
                    string Phonenumber = "+375" + Math.Abs(random.Next()) % 10000000;
                    string PassportId = "HB" + Math.Abs(random.Next()) % 1000000000;
                    db.Add(new Investor() { Name = Name, Surname = Surname, Middlename = Middlename, Address = Address, Phonenumber = Phonenumber, PassportId = PassportId });
                }
                db.SaveChanges();
            }

            if (!db.Currencies.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    string Name = "Валюта " + i;
                    string Country = "Страна " + i;
                    db.Add(new Currency() { Name = Name, Country = Country });
                }
                db.SaveChanges();
            }

            if (!db.Deposits.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    string Name = "Вклад " + i;
                    double Term = random.Next(3,7);
                    decimal Mindepositamount = random.Next(1000, 3000);
                    int Currencyid = random.Next(1, 11);
                    decimal Rate = random.Next(3, 15); 
                    string? Additionalconditions = Additionalconditions1[random.Next(0, Additionalconditions1.Count)];
                    db.Add(new Deposit() { Name = Name, Term = Term, Mindepositamount = Mindepositamount, CurrencyId = Currencyid, Rate = Rate, Additionalconditions = Additionalconditions });
                }
                db.SaveChanges();
            }

            if (!db.Exchangerates.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    DateTime Date = DateTime.Now.Date;
                    int Currencyid = random.Next(1, 11);
                    decimal Cost = random.Next(30, 101);
                    db.Add(new Exchangerate() { Date = Date, CurrencyId = Currencyid, Cost = Cost });
                }
                db.SaveChanges();
            }

            if (!db.Operations.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    int Investorsid = random.Next(1, 101);
                    DateTime Depositdate = DateTime.Now.AddDays(-1*random.Next(60, 100));
                    DateTime Returndate = DateTime.Now.AddDays(random.Next(60, 100));
                    int Depositid = random.Next(1, 101);
                    decimal Depositamount = random.Next(3001, 4501); 
                    decimal Refundamount = random.Next(300, 1000); 
                    bool Returnstamp = false;
                    int Emploeeid = random.Next(1, 101); 
                    db.Add(new Operation() { InvestorId = Investorsid, Depositdate = Depositdate, Returndate = Returndate, DepositId = Depositid, Depositamount = Depositamount, Refundamount = Refundamount, Returnstamp = Returnstamp, EmploeeId = Emploeeid });
                }
                db.SaveChanges();
            }

            
            





        }
    }
}
