using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Fundamentals;

namespace TestApp.Mocking
{
    public class VehiclesViewModel
    {
        public ICollection<Vehicle> Vehicles { get; private set; }

        public VehicleSearchCriteria Criteria { get; set; }

        public ICommand SearchCommand { get; set; }

        private readonly IVehicleRepository vehicleRepository;

        public VehiclesViewModel()
        {
            this.Criteria = new VehicleSearchCriteria();
              
            SearchCommand = new RelayCommand(async () => await SearchAsync(), () => CanSearch);    
        }

        public async Task SearchAsync()
        {
            this.Vehicles = await vehicleRepository.GetAsync(Criteria);
        }
        
        public bool CanSearch => true;

    }

    public interface IEntityRepository<TEntity>
    {
        ICollection<TEntity> Get();
        TEntity Get(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(int id);
    }

    public interface IVehicleRepository : IEntityRepository<Vehicle>
    {
        Task<ICollection<Vehicle>> GetAsync(VehicleSearchCriteria criteria);
    }

    public class VehicleSearchCriteria
    {
        public string Name { get; set; }
        public string Model { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Model);
    }

    public interface ICommand
    {
        event EventHandler CanExecuteChanged;

        void Execute(object parameter);
        bool CanExecute(object parameter);
    }

    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            //if (execute!=null)
            //    execute.Invoke();

            execute?.Invoke();
        }

        public void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
