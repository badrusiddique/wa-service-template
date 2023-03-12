using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wati.Template.Common.Configurations;

public class AuthenticationConfiguration
{
    public bool IsEnabled { get; set; }
    public bool SkipCertificateCheck { get; set; }
}