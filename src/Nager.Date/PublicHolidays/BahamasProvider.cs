using Nager.Date.Contract;
using Nager.Date.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nager.Date.PublicHolidays
{
    /// <summary>
    /// Bahamas
    /// </summary>
    public class BahamasProvider : IPublicHolidayProvider
    {
        private readonly ICatholicProvider _catholicProvider;

        /// <summary>
        /// BahamasProvider
        /// </summary>
        /// <param name="catholicProvider"></param>
        public BahamasProvider(ICatholicProvider catholicProvider)
        {
            this._catholicProvider = catholicProvider;
        }

        ///<inheritdoc/>
        public IEnumerable<PublicHoliday> Get(int year)
        {
            var countryCode = CountryCode.BS;

            var firstFridayInJune = DateSystem.FindDay(year, Month.June, DayOfWeek.Friday, Occurrence.First);
            var firstMondayInAugust = DateSystem.FindDay(year, Month.August, DayOfWeek.Monday, Occurrence.First);
            var secondMondayInOctober = DateSystem.FindDay(year, Month.October, DayOfWeek.Monday, Occurrence.Second);

            var items = new List<PublicHoliday>();
            items.Add(this.ApplyShiftingRules(new PublicHoliday(year, 1, 1, "New Year's Day", "New Year's Day", countryCode)));
            items.Add(this.ApplyShiftingRules(new PublicHoliday(year, 1, 10, "Majority Rule Day", "Majority Rule Day", countryCode)));
            items.Add(this.ApplyShiftingRules(this._catholicProvider.GoodFriday("Good Friday", year, countryCode)));
            items.Add(this.ApplyShiftingRules(this._catholicProvider.EasterMonday("Easter Monday", year, countryCode)));
            items.Add(this.ApplyShiftingRules(this._catholicProvider.WhitMonday("Whit Monday", year, countryCode)));
            // no source found for this day, neither on https://www.bahamashclondon.net/consular-information/public-holidays/ nor on https://www.bahamasmaritime.com/news-events/public-holidays/ or wikipedia 
            //items.Add(this.ApplyShiftingRules(new PublicHoliday(year, 4, 1, "Perry Christie Day", "Perry Christie Day", countryCode)));
            items.Add(new PublicHoliday(firstFridayInJune, "Randol Fawkes", "Labour Day", countryCode, 1961));
            items.Add(new PublicHoliday(year, 7, 10, "Independence Day", "Independence Day", countryCode));
            items.Add(new PublicHoliday(firstMondayInAugust, "Emancipation Day", "Emancipation Day", countryCode));
            items.Add(new PublicHoliday(secondMondayInOctober, "National Heroes' Day", "National Heroes' Day", countryCode));
            items.Add(new PublicHoliday(year, 12, 25, "Christmas Day", "Christmas Day", countryCode));
            items.Add(new PublicHoliday(year, 12, 26, "Boxing Day", "St. Stephen's Day", countryCode));

            return items.OrderBy(o => o.Date);
        }

        private PublicHoliday ApplyShiftingRules(PublicHoliday holiday)
        {
            return holiday
                .Shift(saturday => saturday.AddDays(2), sunday => sunday.AddDays(1))
                .ShiftWeekdays(tuesday: tuesday => tuesday.AddDays(-1), wednesday: wednesday => wednesday.AddDays(2), thursday: thursday => thursday.AddDays(1));
        }

        ///<inheritdoc/>
        public IEnumerable<string> GetSources()
        {
            return new string[]
            {
                "https://en.wikipedia.org/wiki/Public_holidays_in_the_Bahamas",
                "https://www.bahamashclondon.net/consular-information/public-holidays/",
                "https://bs.usembassy.gov/holiday-calendar/",
                "https://www.bahamasmaritime.com/news-events/public-holidays/"
            };
        }
    }
}
