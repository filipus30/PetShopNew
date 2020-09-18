﻿using System;
using System.Collections.Generic;
using Petshop.Core.Entity;

namespace Petshop.Core.ApplicationService
{
    
        public interface IOwnerService
        {
        public List<Owner> GetAllOwners();
        public List<Owner> SearchForOwner(int toSearchInt, string searchValue);
        public Owner AddNewOwner(string firstname, string lastname, string address, string phonenr, string email);
        public List<Owner> FindOwnersByName(string theName);
        public Owner FindOwnerByID(int theId);
        public Owner UpdateOwner(Owner updatedOwner, int toUpdateInt, string updateValue);
        public Owner DeleteOwnerByID(int theId);
        public List<Pet> FindAllPetsByOwner(Owner theOwner);
    }
    
}
