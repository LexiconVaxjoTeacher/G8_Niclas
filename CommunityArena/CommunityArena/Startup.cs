﻿using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityArena
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}