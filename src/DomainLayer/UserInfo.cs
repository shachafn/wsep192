
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
        if (IsDigitsOnly(phoneNumber))
        {
            this.phoneNumber = phoneNumber;
        }
        this.fullName = fullName;
        this.address = address;
    }

    public string phoneNumber { get => phoneNumber; set => phoneNumber = IsDigitsOnly(phoneNumber)? phoneNumber :""; }
    public string fullName { get => fullName; set => fullName = value; }
    public string address { get => address; set => address = value; }

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
