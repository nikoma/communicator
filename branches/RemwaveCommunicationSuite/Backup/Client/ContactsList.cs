using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace Remwave.Client
{
   
   public enum StatusImage
   {
      Green,
      Yellow,
      Red
   }

   public class ContactsListCell : DataGridViewImageCell
   {
      public ContactsListCell()
      {
         this.ImageLayout = DataGridViewImageCellLayout.Zoom;
      }

      protected override object GetFormattedValue(object value,
         int rowIndex, ref DataGridViewCellStyle cellStyle,
         TypeConverter valueTypeConverter,
         TypeConverter formattedValueTypeConverter,
         DataGridViewDataErrorContexts context)
      {

         string resource = "CustomColumnAndCell.Red.bmp";
         StatusImage status = StatusImage.Red;
         // Try to get the default value from the containing column
         ContactsListColumn owningCol = OwningColumn as ContactsListColumn;
         if (owningCol != null)
         {
            status = owningCol.DefaultStatus;
         }
         if (value is StatusImage || value is int)
         {
            status = (StatusImage)value;
         }
         switch (status)
         {
            case StatusImage.Green:
                resource = "RemwaveClient.Resources.ContactBlank.png";
               break;            
             case StatusImage.Yellow:
                 resource = "RemwaveClient.Resources.ContactBlank.png";
               break;
            case StatusImage.Red:
                resource = "RemwaveClient.Resources.ContactBlank.png";
               break;
            default:
               break;
         }
         Assembly loadedAssembly = Assembly.GetExecutingAssembly();
         string[] mylist  = loadedAssembly.GetManifestResourceNames();

         Stream stream =
            loadedAssembly.GetManifestResourceStream(resource);
         Image img = Image.FromStream(stream);
         cellStyle.Alignment =
            DataGridViewContentAlignment.TopCenter;
         return img;
      }
   }
    public class ContactsListColumn : DataGridViewColumn
   {
      public ContactsListColumn() : base(new ContactsListCell())
      {
      }
      private StatusImage m_DefaultStatus = StatusImage.Red;

      public StatusImage DefaultStatus
      {
         get { return m_DefaultStatus; }
         set { m_DefaultStatus = value; }
      }

      public override object Clone()
      {
          ContactsListColumn col = base.Clone() as ContactsListColumn;
         col.DefaultStatus = m_DefaultStatus;
         return col;
      }

      public override DataGridViewCell CellTemplate
      {
         get { return base.CellTemplate; }
         set
         {
             if ((value == null) || !(value is ContactsListCell))
           {
              throw new ArgumentException(
   "Invalid cell type, StatusColumns can only contain StatusCells");
               }
            }
        }
     }
}

