using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

namespace PawPal.ViewModel;

public class MedicalRecordsViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public int SelectedPetId { get; set; }
    public ObservableCollection<MedicalRecord> MedicalRecords { get; private set; } = [];
    public MedicalRecord? SelectedMedicalRecord { get; private set; }
    public MedicalRecord EditableMedicalRecord { get; private set; } = new();
    public bool IsFormVisible { get; private set; }

    public ICommand AddMedicalRecordCommand { get; }
    public ICommand UpdateMedicalRecordCommand { get; }
    public ICommand DeleteMedicalRecordCommand { get; }
    public ICommand CancelEditCommand { get; }
    public ICommand SaveMedicalRecordCommand { get; }

    public MedicalRecordsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AddMedicalRecordCommand = new Command(ShowAddMedicalRecordForm);
        UpdateMedicalRecordCommand = new Command(ShowUpdateMedicalRecordForm, () => SelectedMedicalRecord != null);
        DeleteMedicalRecordCommand = new Command(DeleteMedicalRecord, () => SelectedMedicalRecord != null);
        CancelEditCommand = new Command(CancelEdit);
        SaveMedicalRecordCommand = new Command(SaveMedicalRecord);
    }

    public async void LoadMedicalRecords()
    {
        if (SelectedPetId > 0)
        {
            var records = await _databaseService.GetMedicalRecordsForPetAsync(SelectedPetId);
            MedicalRecords = [.. records];
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
            await _databaseService.InsertMedicalRecordAsync(EditableMedicalRecord);
        else
            await _databaseService.UpdateMedicalRecordAsync(EditableMedicalRecord);

        LoadMedicalRecords();
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
            await _databaseService.DeleteMedicalRecordAsync(SelectedMedicalRecord);
            LoadMedicalRecords();
        }
    }
}
