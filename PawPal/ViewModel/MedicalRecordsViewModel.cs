using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PawPal.Data;
using PawPal.Models;

namespace PawPal.ViewModel
{
    public class MedicalRecordsViewModel : BaseViewModel
    {
        private readonly AppDataContext _context;

        public int SelectedPetId { get; set; }
        public ObservableCollection<MedicalRecord> MedicalRecords { get; private set; } = [];
        public MedicalRecord? SelectedMedicalRecord { get; private set; }
        public MedicalRecord EditableMedicalRecord { get; private set; } = new MedicalRecord();
        public bool IsFormVisible { get; private set; }

        public ICommand AddMedicalRecordCommand { get; }
        public ICommand UpdateMedicalRecordCommand { get; }
        public ICommand DeleteMedicalRecordCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand SaveMedicalRecordCommand { get; }

        public MedicalRecordsViewModel(AppDataContext context)
        {
            _context = context;

            AddMedicalRecordCommand = new Command(ShowAddMedicalRecordForm);
            UpdateMedicalRecordCommand = new Command(ShowUpdateMedicalRecordForm, () => SelectedMedicalRecord != null);
            DeleteMedicalRecordCommand = new Command(DeleteMedicalRecord, () => SelectedMedicalRecord != null);
            CancelEditCommand = new Command(CancelEdit);
            SaveMedicalRecordCommand = new Command(SaveMedicalRecord);
        }

        public async Task LoadMedicalRecords()
        {
            if (SelectedPetId > 0)
            {
                var records = await _context.MedicalRecords.Where(r => r.PetId == SelectedPetId).ToListAsync();
                MedicalRecords.Clear();
                foreach (var record in records)
                {
                    MedicalRecords.Add(record);
                }
                OnPropertyChanged(nameof(MedicalRecords));
            }
        }

        private void ShowAddMedicalRecordForm()
        {
            EditableMedicalRecord = new MedicalRecord { PetId = SelectedPetId, RecordDate = DateTime.Now };
            IsFormVisible = true;
            OnPropertyChanged(nameof(EditableMedicalRecord));
            OnPropertyChanged(nameof(IsFormVisible));
        }

        private void ShowUpdateMedicalRecordForm()
        {
            if (SelectedMedicalRecord != null)
            {
                EditableMedicalRecord = new MedicalRecord
                {
                    Id = SelectedMedicalRecord.Id,
                    PetId = SelectedMedicalRecord.PetId,
                    RecordDate = SelectedMedicalRecord.RecordDate,
                    RecordType = SelectedMedicalRecord.RecordType,
                    Notes = SelectedMedicalRecord.Notes,
                    Prescriptions = SelectedMedicalRecord.Prescriptions,
                    Doctor = SelectedMedicalRecord.Doctor
                };
                IsFormVisible = true;
                OnPropertyChanged(nameof(EditableMedicalRecord));
                OnPropertyChanged(nameof(IsFormVisible));
            }
        }

        private async void SaveMedicalRecord()
        {
            if (EditableMedicalRecord.Id == 0)
            {
                await _context.MedicalRecords.AddAsync(EditableMedicalRecord); // Add new medical record
            }
            else
            {
                _context.MedicalRecords.Update(EditableMedicalRecord); // Update existing medical record
            }
            await _context.SaveChangesAsync();
            await LoadMedicalRecords(); // Refresh the medical records list after save
            IsFormVisible = false;
            OnPropertyChanged(nameof(IsFormVisible));
        }

        private void CancelEdit()
        {
            IsFormVisible = false;
            OnPropertyChanged(nameof(IsFormVisible));
        }

        private async void DeleteMedicalRecord()
        {
            if (SelectedMedicalRecord != null)
            {
                _context.MedicalRecords.Remove(SelectedMedicalRecord); // Remove selected record
                await _context.SaveChangesAsync();
                await LoadMedicalRecords(); // Refresh the medical records list after deletion
            }
        }
    }
}
