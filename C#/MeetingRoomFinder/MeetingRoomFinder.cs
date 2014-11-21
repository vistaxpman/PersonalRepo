using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Outlook;

namespace MeetingRoomFinder
{
    class MeetingRoomFinder
    {
        static void Main(string[] args)
        {
            DayOfWeek dayOfWeek;
            TimeSpan startTime;
            TimeSpan duration;
            int count;
            int nextWeeks;

            ParseCommandlineArguments(args, out dayOfWeek, out startTime, out duration, out count, out nextWeeks);
            FindAvailableRooms(GetAllMeetingRooms(), dayOfWeek, startTime, duration, count, nextWeeks);
        }

        private static void ParseCommandlineArguments(string[] args, out DayOfWeek dayOfWeek, out TimeSpan startTime,
            out TimeSpan duration, out int count, out int nextWeeks)
        {
            dayOfWeek = DayOfWeek.Sunday;
            startTime = TimeSpan.Zero;
            duration = TimeSpan.Zero;
            count = 1;
            nextWeeks = 0;

            try
            {
                if (args.Length < 3)
                {
                    throw new ArgumentException("The arguments is less than 3.");
                }

                if (!Enum.TryParse<DayOfWeek>(args[0], out dayOfWeek))
                {
                    throw new ArgumentException("The argument 'dayOfWeek' isn't valid.");
                }

                if (!TimeSpan.TryParse(args[1], out startTime))
                {
                    throw new ArgumentException("The argument 'startTime' isn't valid.");
                }

                int durationMinutes;
                if (!int.TryParse(args[2], out durationMinutes))
                {
                    throw new ArgumentException("The argument 'duration' isn't valid.");
                }
                duration = TimeSpan.FromMinutes(durationMinutes);

                if ((args.Length > 3) &&
                    !int.TryParse(args[3], out count) || (count <= 0))
                {
                    throw new ArgumentException("The argument 'count' isn't valid.");
                }
                if (count > 99)
                {
                    throw new ArgumentException("The argument 'count' is not greater than 99.");
                }

                if ((args.Length > 4) &&
                    (!int.TryParse(args[4], out nextWeeks) || (nextWeeks < 0)))
                {
                    throw new ArgumentException("The argument 'nextWeeks' isn't valid.");
                }
            }
            catch (ArgumentException e)
            {
                Usage(e);
                Environment.Exit(-1);
            }
        }

        private static void Usage(ArgumentException e)
        {
            var usage = "MeetingRoomFinder.exe dayOfWeek startTime duration count nextWeeks\n" +
                "dayOfWeek: 0 -6 or Sunday - Saturday\n" +
                "startTime: 9:00\n" +
                "duration:  minutes\n" +
                "count:     occurrence times\n" +
                "nextWeeks: the 1st week for this meeting\n";
            Console.WriteLine(usage);
            if (e != null)
            { 
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        private static void FindAvailableRooms(IEnumerable<AddressEntry> meetingRooms, DayOfWeek dayOfWeek,
            TimeSpan startTime, TimeSpan duration, int count, int nextWeeks)
        {
            var today = DateTime.Today;
            var dayofWeekInterval = TimeSpan.FromDays((double)(dayOfWeek - DayOfWeek.Sunday - today.DayOfWeek));
            var WeekInterval = TimeSpan.FromDays(7 * nextWeeks);
            var startDatetime = today + dayofWeekInterval + WeekInterval + startTime;

            for (int i = 1; i <= count; i++, startDatetime += TimeSpan.FromDays(7))
            {
                int totalRoomNumber = meetingRooms.Count();
                meetingRooms = (from r in meetingRooms
                                where IsAvailable(r, startDatetime, duration)
                                select r).ToArray();
                Console.WriteLine("{0, -2} {1}: {2} Available Rooms from {3} rooms", i, startDatetime,
                    meetingRooms.Count(), totalRoomNumber);

                foreach (var r in meetingRooms)
                {
                    Console.WriteLine(r.Name);
                }
                Console.WriteLine();
            };
        }

        private static IEnumerable<AddressEntry> GetAllMeetingRooms()
        {
            var application = new Application();
            foreach (AddressList list in application.Session.AddressLists)
            {
                if (list.Name != "All Rooms")
                {
                    continue;
                }

                return from AddressEntry r in list.AddressEntries
                       where IsNormalMeetingRoom(r)
                       select r;
            }
            return new List<AddressEntry>();
        }

        private static bool IsNormalMeetingRoom(AddressEntry room)
        {
            var roomName = room.Name;
            if (!roomName.StartsWith("CN BJS (ARCA"))
            {
                return false;
            }

            if (roomName.Contains("Training"))
            {
                return false;
            }

            if (roomName.Contains("Imperial Garden"))
            {
                return false;
            }

            if (roomName.Contains("Grand View Park"))
            {
                return false;
            }

            return true;
        }

        private static bool IsAvailable(AddressEntry entry, DateTime startTime, TimeSpan duration)
        {
            int length = (int)duration.TotalMinutes;
            int index = (int)startTime.TimeOfDay.TotalMinutes;
            string freeBusyResult = entry.GetFreeBusy(startTime, 1);
            return !freeBusyResult.Substring(index, length).Contains("1");
        }

    }
}