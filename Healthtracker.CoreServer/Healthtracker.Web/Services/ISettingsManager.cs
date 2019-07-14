using System;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services
{
    public interface ISettingsManager
    {
        string GetSetting(AuthSetting setting);
    }
}
