using System.Collections.ObjectModel;
using System.Windows.Input;
using PawPal.Models;
using PawPal.Services;

namespace PawPal.ViewModel;

public class MedicalRecordsViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    private int _selectedPetId;
    public int SelectedPetId
    {
        get => _selectedPetId;
        set
        {
            if (SetProperty(ref _selectedPetId, value))
            {
                LoadMedicalRecords();
            }
        }
    }

    private ObservableCollection<MedicalRecord> _medicalRecords = [];
    public ObservableCollection<MedicalRecord> MedicalRecords
    {
        get => _medicalRecords;
        set => SetProperty(ref _medicalRecords, value);
    }

    private MedicalRecord? _selectedMedicalRecord;
    public MedicalRecord? SelectedMedicalRecord
    {
        get => _selectedMedicalRecord;
        set
        {
            if (SetProperty(ref _selectedMedicalRecord, value))
            {
                // Notify the commands to recheck their CanExecute status
                ((Command)UpdateMedicalRecordCommand).ChangeCanExecute();
                ((Command)DeleteMedicalRecordCommand).ChangeCanExecute();
            }
        }
    }

    private MedicalRecord _editableMedicalRecord = new();
    public MedicalRecord EditableMedicalRecord
    {
        get => _editableMedicalRecord;
        set => SetProperty(ref _editableMedicalRecord, value);
    }

    private bool _isFormVisible;
    public bool IsFormVisible
    {
        get => _isFormVisible;
        set => SetProperty(ref _isFormVisible, value);
    }

    public ICommand AddMedicalRecordCommand { get; }
    public ICommand UpdateMedicalRecordCommand { get; }
    public ICommand DeleteMedicalRecordCommand { get; }
    public ICommand CancelEditCommand { get; }
    public ICommand SaveMedicalRecordCommand { get; }

    public MedicalRecordsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AddMedicalRecordCommand = new Command(ShowAddMedicalRecordForm);
        UpdateMedicalRecordCommand = new Command(ShowUpdateMedicalRecordForm, CanExecuteMedicalRecordCommand);
        DeleteMedicalRecordCommand = new Command(DeleteMedicalRecord, CanExecuteMedicalRecordCommand);
        CancelEditCommand = new Command(CancelEdit);
        SaveMedicalRecordCommand = new Command(SaveMedicalRecord);
    }

    private async void LoadMedicalRecords()
    {
        if (SelectedPetId > 0)
        {
            var records = await _databaseService.GetMedicalRecordsForPetAsync(SelectedPetId);
            MedicalRecords = [.. records];
        }
    }

    private void ShowAddMedicalRecordForm()
    {
        EditableMedicalRecord = new MedicalRecord
        {
            PetId = SelectedPetId,
            RecordDate = DateTime.Now
        };
        IsFormVisible = true;
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
        }
    }

    private void SaveMedicalRecord()
    {
        if (EditableMedicalRecord.Id == 0)
        {
            // Add new record
            _databaseService.InsertMedicalRecordAsync(EditableMedicalRecord);
        }
        else
        {
            // Update existing record
            _databaseService.UpdateMedicalRecordAsync(EditableMedicalRecord);
        }

        LoadMedicalRecords();
        IsFormVisible = false;
    }

    private void CancelEdit()
    {
        IsFormVisible = false;
    }

    private void DeleteMedicalRecord()
    {
        if (SelectedMedicalRecord != null)
        {
            _databaseService.DeleteMedicalRecordAsync(SelectedMedicalRecord);
            LoadMedicalRecords();
        }
    }

    private bool CanExecuteMedicalRecordCommand()
    {
        return SelectedMedicalRecord != null;
    }
}
