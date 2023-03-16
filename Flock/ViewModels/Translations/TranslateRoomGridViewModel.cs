using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flock.Models;

namespace Flock.ViewModels
{
    public class TranslateRoomGridViewModel
    {
        public List<Room_Language_SelectList_RoomResult> Rooms { get; set; }
        public byte? TargetLanguageID { get; set; }
        
        public TranslateRoomGridViewModel()
        {
            Rooms = new List<Room_Language_SelectList_RoomResult>();
        }

        public TranslateRoomGridViewModel(
            List<Room_Language_SelectList_RoomResult> rooms,
            byte? targetLanguageId)
        {
            Rooms = rooms;
            TargetLanguageID = targetLanguageId;
        }
    }
}