﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }
        //// TODO Items: ////
        // CRUD - create(insert), read(select), update, delete
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            switch (crudOperation)
            {
                case "create":
                db.Employees.InsertOnSubmit(employee);
                db.SubmitChanges();
                break;
                case "read":                
                var employeeTraits = db.Employees.Where(a => a.EmployeeNumber == employee.EmployeeNumber).First();
                Console.WriteLine("Employee ID " + employeeTraits.EmployeeId + ' ' + "Employee Number " + employee.EmployeeNumber);
                List<string> listOfTraits = new List<string>() { employeeTraits.FirstName, employeeTraits.LastName, employeeTraits.UserName, employeeTraits.Password, employeeTraits.Email };
                UserInterface.DisplayUserOptions(listOfTraits);
                Console.ReadLine();
                break;
                case "update":
                employeeTraits = db.Employees.Where(a => a.EmployeeNumber == employee.EmployeeNumber).First();
                GetUpdates(employeeTraits);

                break;
                case "delete":
                employee = db.Employees.Where(e => e.LastName == employee.LastName && e.EmployeeNumber == employee.EmployeeNumber).SingleOrDefault();
                db.Employees.DeleteOnSubmit(employee);
                db.SubmitChanges();
                break;
            }
        }
        internal static void GetUpdates(Employee employee)
        {
            List<string> updtes = new List<string>();
            UserInterface.DisplayUserOptions("What would you like to update use spaces!!");
            
            List<string> options = new List<string>() {"First Name ", "Last Name ", "User Name ", "Password ", "Employee Number ", "Email " };
            UserInterface.DisplayUserOptions(options);

            bool stillChosing = true;
            while (stillChosing == true)
            {
                updtes.Add(Console.ReadLine());
                Console.WriteLine("Want to continue? y/n");
                string answer = Console.ReadLine();
                if(answer == "n")
                {
                    stillChosing = false;
                }
            }
            ApplyUpdates(updtes, employee);
            
        }
        public static void ApplyUpdates(List<string> updates, Employee employee )
        {
            foreach(string update in updates)
            {
                switch (update)
                {
                    case "First Name":
                    Console.WriteLine("What do you want to change the First Name to?");
                    employee.FirstName = Console.ReadLine();
                    db.SubmitChanges();
                    break;

                    case "Last Name":
                    Console.WriteLine("What do you want to change the Last Name to?");
                    employee.LastName = Console.ReadLine();
                    db.SubmitChanges();
                    break;
                  
                    case "User Name":
                    Console.WriteLine("What do you want to change the User Name to?");
                    employee.UserName = Console.ReadLine();
                    db.SubmitChanges();
                    break;

                    case "Password":
                    Console.WriteLine("What do you want to change the Password to?");
                    employee.Password = Console.ReadLine();
                    db.SubmitChanges();
                    break;

                    case "Employee Number":
                    Console.WriteLine("What do you want to change the Employee Number to?");
                    employee.EmployeeNumber = Int32.Parse(Console.ReadLine());
                    db.SubmitChanges();
                    break;

                    case "Email":
                    Console.WriteLine("What do you want to change the Email to?");
                    employee.Email = Console.ReadLine();
                    db.SubmitChanges();
                    break;

                    default:
                    break;
                }
            }
        }
        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }
        internal static Animal GetAnimalByID(int id)
        {
            Animal result = new Animal();
            result = db.Animals.Where(a => a.AnimalId == id).FirstOrDefault();
            return result;
        }
        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            Animal animal = GetAnimalByID(animalId);
            foreach (KeyValuePair<int,string> item in updates)
            {
                switch (item.Key)
                {
                    case 1:
                    int cateID = db.Categories.Where(s => s.Name == item.Value).Single().CategoryId;                    
                    animal.CategoryId = cateID;
                    db.SubmitChanges();
                    break;
                    case 2:
                    animal.Name = item.Value;
                    db.SubmitChanges();
                    break;
                    case 3:
                    animal.Age = Int32.Parse(item.Value);
                    db.SubmitChanges();
                    break;
                    case 4:
                    animal.Demeanor = item.Value;
                    db.SubmitChanges();
                    break;
                    case 5:
                    animal.KidFriendly = bool.Parse(item.Value);
                    db.SubmitChanges();
                    break;
                    case 6:
                    animal.PetFriendly = bool.Parse(item.Value);
                    db.SubmitChanges();
                    break;
                    case 8:
                    animal.Weight = Int32.Parse(item.Value);
                    db.SubmitChanges();
                    break;
                }
            }
        }

        internal static void RemoveAnimal(Animal animal)
        {
                db.Animals.DeleteOnSubmit(animal);
                db.SubmitChanges();
        }

        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            var results = db.Animals.Select(a=>a);
            foreach (KeyValuePair<int, string> traits in updates)
            {
                switch (traits.Key)
                {
                    case 1:
                    results = results.Where(a => a.Category.Name == traits.Value).Select(a=>a);
                    break;
                    case 2:
                    results = results.Where(a => a.Name == traits.Value).Select(a=>a);
                    break;
                    case 3:
                    results = results.Where(a => a.Age == Int32.Parse(traits.Value));
                    break;
                    case 4:
                    results = results.Where(a => a.Demeanor == traits.Value);
                    break;
                    case 5:
                    results = results.Where(a => a.KidFriendly == bool.Parse(traits.Value));
                    break;
                    case 6:
                    results = results.Where(a => a.PetFriendly == bool.Parse(traits.Value));
                    break;
                    case 7:
                    results = results.Where(a => a.Weight == Int32.Parse(traits.Value));
                    break;
                    case 8:
                    results = results.Where(a => a.AnimalId == Int32.Parse(traits.Value));
                    break;
                    default:
                    break;
                }
            }
            return results;
        }

        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            var category = db.Categories.Where(c => c.Name == categoryName).FirstOrDefault();
            return category.CategoryId;
        }
        
        internal static Room GetRoom(int animalId)
        {
            var rooms = db.Rooms.Where(r => r.AnimalId == animalId).FirstOrDefault();
            return rooms;
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            var dietPlan = db.DietPlans.Where(d => d.Name == dietPlanName).FirstOrDefault();
            return dietPlan.DietPlanId;
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            UserInterface.DisplayUserOptions("Who will be adopting today?");
            DisplayAllCustomers();
            UserInterface.DisplayUserOptions("Enter client Id");
            int id = Int32.Parse(Console.ReadLine());
            Client personAdopting = db.Clients.Where(c => c.ClientId == id).FirstOrDefault();
            //add clientId to the Adoptions table
            Console.Clear();

            UserInterface.DisplayUserOptions("What animal is being requested?");
            DisplayAnimals();
            UserInterface.DisplayUserOptions("Enter animal Id");
            int animalId = Int32.Parse(Console.ReadLine());
            Animal animalBeingAdopted = GetAnimalByID(animalId);
            //add animalId to same table
            Console.Clear();

            UserInterface.DisplayUserOptions("What is the adoption status (Approved, UnApproved or Pending)");
            string status = Console.ReadLine();
            //add status to table

            UserInterface.DisplayUserOptions("What is the adoption fee");
            int fee = Int32.Parse(Console.ReadLine());
            //add fee

            UserInterface.DisplayUserOptions("Has this adoption been paid for? (yes or no)");
            string yesorno = Console.ReadLine();
            bool payStatus = UserInterface.GetBitData(yesorno);
            //add status to table
        }

        internal static void DisplayAllCustomers()
        {
            var animals = db.Animals;
            foreach (Animal animal in animals)
            {
                Console.WriteLine(animal.Name + " " + animal.Category + " " + animal.AnimalId);
            }
        }

        internal static void DisplayAnimals()
        {
            var animals = db.Clients;
            foreach (Client animal in animals)
            {
                Console.WriteLine(animal.FirstName + " " + animal.LastName + " " + animal.ClientId);
            }
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            IQueryable<Adoption> adoptions = db.Adoptions.Where(a => a.ApprovalStatus == "Pending");
            return adoptions;
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            var requiredData =
             (from y in db.Adoptions
              where y.ClientId == adoption.ClientId
              select y).First();
            var animal =
                (from z in db.Animals
                 where z.AnimalId == adoption.AnimalId
                 select z).First();
            if (isAdopted)
            {
                requiredData.ApprovalStatus = "Approved";
                animal.AdoptionStatus = "Adopted";
            }
            else
            {
                requiredData.ApprovalStatus = "Denied";
                animal.AdoptionStatus = "Animal is available for a different applicant.";
            }

            db.SubmitChanges();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            //db.Adoptions.DeleteOnSubmit(a);
            //db.SubmitChanges();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            var shot = db.AnimalShots.Where(x => x.AnimalId == animal.AnimalId);
            return shot;
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            var updates = UserInterface.GetAnimalSearchCriteria();
            var animals = Query.SearchForAnimalsByMultipleTraits(updates).ToList();

            while (animals.Count !=1)
            {
                Console.Clear();
                Console.WriteLine("You have to return a single animal");
                updates = UserInterface.GetAnimalSearchCriteria();
                animals = Query.SearchForAnimalsByMultipleTraits(updates).ToList();
            }
            Animal pet = animals[0];
            List<Shot> Shots = db.Shots.ToList();
            UserInterface.DisplayUserOptions("What shot do you need for this animal? (Enter shot name)");
            foreach (Shot shot in Shots)
            {
                UserInterface.DisplayUserOptions(shot.Name);
            }
            string answer = Console.ReadLine();
            Shot shotToUse = db.Shots.Where(s => s.Name == answer).FirstOrDefault();
            Console.Clear();
            UserInterface.DisplayUserOptions("Do you want to give " + pet.Name + " the " + shotToUse.Name + " shot ");
        }
    }
}