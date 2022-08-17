namespace Application.Server.Services;


public class DoucmentGenerater
{


    public async Task<string> CreateDocumentNoAsync(string? lastDocumentNo, string prefix)
    {
        
        if(String.IsNullOrEmpty(lastDocumentNo)) {
            return $"{prefix}-{DateTime.Today.ToString("yy")}-000001";;
        }
        else {
            var lastDocumentNoSplit = lastDocumentNo.Split('-')[2];
            var newDocumentNoNumber = int.Parse(lastDocumentNoSplit) + 1;
            
            if(newDocumentNoNumber.ToString().Length == 1)
            {
                return $"{prefix}-{DateTime.Today.ToString("yy")}-00000{newDocumentNoNumber}";
            }
            else if(newDocumentNoNumber.ToString().Length == 2)
            {
                return $"{prefix}-{DateTime.Today.ToString("yy")}-0000{newDocumentNoNumber}";
            }
            else if(newDocumentNoNumber.ToString().Length == 3)
            {
                return $"{prefix}-{DateTime.Today.ToString("yy")}-000{newDocumentNoNumber}";
            }
            else if(newDocumentNoNumber.ToString().Length == 4)
            {
                return $"{prefix}-{DateTime.Today.ToString("yy")}-00{newDocumentNoNumber}";
            }
            else if(newDocumentNoNumber.ToString().Length == 5)
            {
                return $"{prefix}-{DateTime.Today.ToString("yy")}-0{newDocumentNoNumber}";
            }
            else
            {
                return $"{prefix}-{DateTime.Today.ToString("yy")}-{newDocumentNoNumber}";
            }
        }
        
    }
}