﻿using System;
using System.Collections.Generic;
using Petshop.Core.Entity;

namespace Petshop.Core.DomainService
{
    public interface IOwnerRepository
    {
        public IEnumerable<Owner> ReadOwners();
        public IEnumerable<Owner> FindOwnersByName(string name);
        public Owner FindOwnerById(int id);
        public Owner AddOwner(Owner o);
        public Owner UpdateOwner(int id,Owner owner);
        public Owner RemoveOwner(int id, Owner o);
    }
}
