using System.Text.Json.Serialization;

namespace HorseCalenderv2.classes {
  public class Horse {
    private static uint _nextId = 1;

    [JsonIgnore]
    private string _name = string.Empty;
    
    [JsonIgnore]
    private int _rotationInterval = 0;
    
    [JsonIgnore]
    private uint _id = 0;

    public DateTime LastShoeDate { get; set; }

    [JsonInclude]
    public string Name {
      get => _name;
      set {
        if (string.IsNullOrEmpty(value)) {
          throw new ArgumentException("Name cannot be empty");
        }
        _name = value;
      }
    }

    [JsonInclude]
    public int RotationInterval {
      get => _rotationInterval;
      set {
        if (value < 0) {
          throw new ArgumentException("Rotation interval cannot be negative");
        }
        _rotationInterval = value;
      }
    }

    [JsonInclude]
    public uint Id {
      get => _id;
      private set => _id = value;
    }

    [JsonIgnore]
    public int WeeksSinceLastShoe => (int)(DateTime.Now - LastShoeDate).TotalDays / 7;

    [JsonConstructor]
    public Horse(string name, int rotationInterval, DateTime lastShoeDate, uint id) {
      Name = name;
      RotationInterval = rotationInterval;
      LastShoeDate = lastShoeDate;
      Id = id;
      
      // Update _nextId if this id is higher
      if (id >= _nextId) {
        _nextId = id + 1;
      }
    }

    public Horse(string name, int rotationInterval, DateTime lastShoeDate, uint? id = null) {
      Name = name;
      RotationInterval = rotationInterval;
      LastShoeDate = lastShoeDate;

      if (id.HasValue) {
        Id = (uint)id;
      }
      else {
        Id = _nextId++;
      }
    }

    public void Reset(DateTime date) {
      LastShoeDate = date;
    }
  }
}