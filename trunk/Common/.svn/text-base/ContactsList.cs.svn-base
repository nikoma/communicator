using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Collections;

namespace Remwave.Client
{
    public class ContactList : List<NTContact>
    {
        public ContactList()
        {

        }

        public ContactList getCandidatesForJabberID(string jabberID)
        {
            ContactList result = new ContactList();
            try
            {
                lock (this)
                {
                    for (int i = this.Count - 1; i >= 0; i--)
                    {
                        if (this[i].NTJabberID.Trim().ToUpper() == jabberID.Trim().ToUpper() && this[i].NTDeleted != "true")
                        {
                            result.Add(this[i]);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return result;
        }

        public ContactList getCandidatesForName(string[] keys)
        {
            ContactList result = new ContactList();
            foreach (String search in keys)
            {
                try
                {
                    lock (this)
                    {
                        for (int i = this.Count - 1; i >= 0; i--)
                        {
                            if ((this[i].NTLastName.ToUpper().StartsWith(search.ToUpper()) || this[i].NTFirstName.ToUpper().StartsWith(search.ToUpper())) && this[i].NTDeleted != "true")
                            {
                                result.Add(this[i]);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            return result;
        }

        public ContactList getCandidatesForNumber(string number)
        {
            ContactList result = new ContactList();
            try
            {
                lock (this)
                {
                    for (int i = this.Count - 1; i >= 0; i--)
                    {
                        if (this[i].NTHomeTelephoneNumber.StartsWith(number) && this[i].NTDeleted != "true")
                        {
                            result.Add(this[i]);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return result;
        }

        public ContactList getContactList()
        {
            return this;
        }
    }
}