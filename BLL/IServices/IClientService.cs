

namespace BLL.IServices
{
    public interface IClientService
    {
        void GetDataOfClient(string id);
        void ChangeTariffPlan(string id);
        void ShowMyTariffPlan(string id);
        void ChangePassword(string id);
    }
}