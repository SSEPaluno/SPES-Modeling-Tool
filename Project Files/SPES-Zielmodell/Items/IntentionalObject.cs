﻿using System;
using SPES_Modelverifier_Base.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Items;

namespace SPES_Zielmodell.Items
{
    public class IntentionalObject : Item
    {
        public ActorBoundary BelongingActorBoundary => this.Containers.FirstOrDefault(t => t is ActorBoundary) as ActorBoundary;

    }
}
