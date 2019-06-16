using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ServiceLayer
{
    public class SystemInitializer
    {
        private Dictionary<string, Guid> _results;

        private IServiceFacade _facade;
        private IUnitOfWork _unitOfWork;
        ILogger<SystemInitializer> _logger;
        public SystemInitializer(IServiceFacade facade, IUnitOfWork unitOfWork, ILogger<SystemInitializer> logger)

        {
            _unitOfWork = unitOfWork;
            _results = new Dictionary<string, Guid>();
            _facade = facade;
            _logger = logger;
        }

        public void InitSystemWithFile()
        {

            List<Opertaion> jsonList = readJsonFromInitFile();

            var session = _unitOfWork.Context.StartSession();
            session.StartTransaction();
            if(_unitOfWork.BaseUserRepository.IsUserExistsByUsername("avi")|| _unitOfWork.BaseUserRepository.IsUserExistsByUsername("moti"))
            {
                session.CommitTransaction();
                return;
            }
            try
            {
                runOperations(jsonList);
                session.CommitTransaction();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Exception during system initialization from file.");
                session.AbortTransaction();
            }
        }



        private void runOperations(List<Opertaion> opertaions)

        {

            int i = 1;

            foreach (Opertaion op in opertaions)

            {

                runOperation(op, i);

                i++;

            }

        }



        private void runOperation(Opertaion op, int i)

        {

            switch (op.operationName.ToLower())

            {

                case "register":

                    register(op, i);

                    break;

                case "admin":

                    admin(op);

                    break;

                case "openshop":

                    openshop(op, i);

                    break;

                case "addproduct":

                    addproduct(op);

                    break;

                default:

                    throw new Exception("bad init operation");

            }

        }



        private void addproduct(Opertaion op)

        {

            if (op.args.Count != 6)

            {

                throw new Exception("wrong agument number in system init register");

            }

            var userId = _results[op.args[0]];

            var shopId = _results[op.args[1]];

            var name = op.args[2];

            var category = op.args[3];

            var price = float.Parse(op.args[4]);

            var quantity = int.Parse(op.args[5]);

            ShopProduct product = new ShopProduct(new Product(name, category), price, quantity);

            var shop = _unitOfWork.ShopRepository.FindByIdOrNull(shopId);
            shop.ShopProducts.Add(product);
            _unitOfWork.ShopRepository.Update(shop);

        }

        public bool InitSystem()
        {
            try
            {
                _facade.Initialize(Guid.NewGuid(), "Meni", "moni");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "System Initialization failed.");
                return false;
            }
        }

        private void openshop(Opertaion op, int i)

        {

            if (op.args.Count != 2)

            {

                throw new Exception("wrong agument number in system init register");

            }

            var userId = _results[op.args[0]];

            var shop = new Shop(userId, op.args[1]);

            //ShopsOwned.Add(shop);

            _unitOfWork.ShopRepository.Add(shop);

            _results.Add("r" + i, shop.Guid);

        }



        private void admin(Opertaion op)

        {

            if (op.args.Count != 1)

            {

                throw new Exception("wrong agument number in system init register");

            }

            var userId = _results[op.args[0]];

           var user = _unitOfWork.BaseUserRepository.FindByIdOrNull(userId);
            if(user!=null)
            {
                user.IsAdmin = true;
            }
            _unitOfWork.BaseUserRepository.Update(user);

        }



        private void register(Opertaion op, int i)

        {

            if (op.args.Count != 2)

            {

                throw new Exception("wrong agument number in system init register");

            }

            var userId = _facade.Register(Guid.NewGuid(), op.args[0], op.args[1]);

            _results.Add("r" + i, userId);

        }



        private List<Opertaion> readJsonFromInitFile()

        {
            using (StreamReader r = new StreamReader("../../system_init.json"))

            {

                string json = r.ReadToEnd();

                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<Opertaion>));

                List<Opertaion> operationList = (List<Opertaion>)ser.ReadObject(ms);

                return operationList;

            }

        }



        [DataContract]

        private class Opertaion

        {

            [DataMember]

            internal string operationName;



            [DataMember]

            internal List<string> args;

        }

    }

}