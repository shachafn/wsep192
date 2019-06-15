
using System;

using System.Collections.Generic;



namespace ApplicationCore.Entitites

{

    public class OwnerCandidate

    {

        public Guid OwnerGuid { get; set; }



        public Guid AppointerGuid { get; set; } // Guid.Empty for the creator of the shop.



        public Guid ShopGuid { get; set; }



        public Dictionary<string, Guid> Signatures { get; set; }



        public int signature_target { get; }

        public OwnerCandidate(Guid ownerGuid, Guid shopGuid, Guid appointer, int signatures_required, string appointerUsername)

        {

            OwnerGuid = ownerGuid;

            shopGuid = ShopGuid;

            AppointerGuid = appointer;

            signature_target = signatures_required;

            Signatures = new Dictionary<string, Guid>();

            Signatures.Add(appointerUsername, appointer);

        }

    }

}