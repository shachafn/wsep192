
/*
 * a data clas the contains all of the registered user infomation
 */
public class UserInfo
{
	public UserInfo()
	{

	}
    public UserInfo(string address,string fullName,string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        this.FullName = fullName;
        this.Address = address;
    }

    private string _phoneNumber;
    public string PhoneNumber { get => _phoneNumber; set { _phoneNumber = IsDigitsOnly(value) ? value : ""; } }
    public string FullName { get; set; }
    public string Address { get; set; }

    private bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }

        return true;
    }
}
