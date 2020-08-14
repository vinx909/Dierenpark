using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dierenpark
{
    public partial class Form1 : Form
    {
        private const string currentDateLabelText = "current date: ";
        private const string addBirthdateButtonText = "add a person";
        private const string calculateSubscriptionButtonText = "calculate Subcription price";

        private const int widthMargin = 10;
        private const int heightMargin = 10;
        private const int rowHeight = 30;

        private const int addBirthdateButtonWidth = 100;
        private const int calculateSubscriptionButtonWidth = 150;

        private Date currentDate;
        private List<BirthDate> birthDates;
        private Button addBirthdateButton;
        private Button calculateSubscriptionButton;

        public Form1()
        {
            InitializeComponent();
            CurrentDateInitialize();
            BirthDatesInitialize();
            AddBirthdateButtonInitialize();
            CalculateSubscriptionInitialize();
            ResetPositions();
        }

        private void CurrentDateInitialize()
        {
            currentDate = new Date(this, currentDateLabelText);
        }
        private void BirthDatesInitialize()
        {
            birthDates = new List<BirthDate>();
        }
        private void BirthDatesAdd()
        {
            birthDates.Add(new BirthDate(this));
        }
        internal void BirthDatesRemove(BirthDate toRemove)
        {
            birthDates.Remove(toRemove);
        }
        private void AddBirthdateButtonInitialize()
        {
            addBirthdateButton = new Button();
            addBirthdateButton.Width = addBirthdateButtonWidth;
            addBirthdateButton.Text = addBirthdateButtonText;
            addBirthdateButton.Click += new EventHandler(ButtonFunctionAddBithdateButton);
            Controls.Add(addBirthdateButton);
        }
        private void CalculateSubscriptionInitialize()
        {
            calculateSubscriptionButton = new Button();
            calculateSubscriptionButton.Width = calculateSubscriptionButtonWidth;
            calculateSubscriptionButton.Text = calculateSubscriptionButtonText;
            calculateSubscriptionButton.Click += new EventHandler(ButtonFunctionCalculateSubscription);
            Controls.Add(calculateSubscriptionButton);
        }

        internal void ResetPositions()
        {
            int rowNumber = 0;
            currentDate.ChangePosition(widthMargin, heightMargin + rowHeight * rowNumber);
            rowNumber++;
            foreach(BirthDate birthDate in birthDates)
            {
                birthDate.ChangePosition(widthMargin, heightMargin + rowHeight * rowNumber);
                rowNumber++;
            }
            addBirthdateButton.Location = new Point(widthMargin, heightMargin + rowHeight * rowNumber);
            rowNumber++;
            calculateSubscriptionButton.Location = new Point(widthMargin, heightMargin + rowHeight * rowNumber);
        }

        private void ButtonFunctionAddBithdateButton(object sender, EventArgs e)
        {
            BirthDatesAdd();
            ResetPositions();
        }
        private void ButtonFunctionCalculateSubscription(object sender, EventArgs e)
        {
            int minAdultAge = CalculateSubscriptions.GetMinAdultAge();
            int minElderlyAge = CalculateSubscriptions.GetMinElderlyAge();
            int childArrayPosition = CalculateSubscriptions.GetChildArrayPosition();
            int adultArrayPosition = CalculateSubscriptions.GetAdultArrayPosition();
            int elderlyArrayPosition = CalculateSubscriptions.GetElderlyArrayPosition();

            int yearIndex = Date.GetYearIndex();
            int monthIndex = Date.GetMonthIndex();
            int dayIndex = Date.GetDayIndex();

            int[] peopleCount = new int[CalculateSubscriptions.GetArraySize()];
            for(int i = 0;i< peopleCount.Length; i++)
            {
                peopleCount[i] = 0;
            }

            int[] compareToDate = currentDate.GetDate();

            foreach(BirthDate birthDate in birthDates)
            {
                int[] date = birthDate.GetDate();
                if (date[yearIndex] + minElderlyAge < compareToDate[yearIndex] || (date[yearIndex] + minElderlyAge <= compareToDate[yearIndex] && date[monthIndex] < compareToDate[monthIndex]) || (date[yearIndex] + minElderlyAge <= compareToDate[yearIndex] && date[monthIndex] <= compareToDate[monthIndex] && date[dayIndex] <= compareToDate[dayIndex]))
                {
                    peopleCount[elderlyArrayPosition]++;
                }
                else if (date[yearIndex] + minAdultAge < compareToDate[yearIndex] || (date[yearIndex] + minAdultAge <= compareToDate[yearIndex] && date[monthIndex] < compareToDate[monthIndex]) || (date[yearIndex] + minAdultAge <= compareToDate[yearIndex] && date[monthIndex] <= compareToDate[monthIndex] && date[dayIndex] <= compareToDate[dayIndex]))
                {
                    peopleCount[adultArrayPosition]++;
                }
                else
                {
                    peopleCount[childArrayPosition]++;
                }
            }
            MessageBox.Show(CalculateSubscriptions.GetSubscriptionsString(peopleCount));
        }

        internal class Date
        {
            protected const int textBoxOfset = 100;
            protected const int textBoxBetweenOfset = 30;
            protected const int textBoxWidth = 25;

            protected const int dateArrayLength = 3;
            protected const int dateDayIndex = 0;
            protected const int dateMonthIndex = 1;
            protected const int dateYearIndex = 2;

            protected Label label;
            protected TextBox textBoxDay;
            protected TextBox textBoxMonth;
            protected TextBox textBoxYear;

            protected Form1 form;

            internal Date(Form1 form, string labelText)
            {
                this.form = form;

                label = new Label();
                label.Text = labelText;
                form.Controls.Add(label);

                textBoxDay = new TextBox();
                textBoxDay.Width = textBoxWidth;
                form.Controls.Add(textBoxDay);

                textBoxMonth = new TextBox();
                textBoxMonth.Width = textBoxWidth;
                form.Controls.Add(textBoxMonth);

                textBoxYear = new TextBox();
                textBoxYear.Width = textBoxWidth;
                form.Controls.Add(textBoxYear);
            }

            internal void ChangePosition(int widthOfset, int heightOfset)
            {
                CorrectPosition(widthOfset, heightOfset);
            }
            protected void CorrectPosition(int widthOfset, int heightOfset)
            {
                label.Location = new Point(widthOfset, heightOfset);
                textBoxDay.Location = new Point(widthOfset + textBoxOfset, heightOfset);
                textBoxMonth.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset, heightOfset);
                textBoxYear.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset * 2, heightOfset);
            }
            internal int[] GetDate()
            {
                int[] date = new int[dateArrayLength];
                date[dateDayIndex] = int.Parse(textBoxDay.Text);
                date[dateMonthIndex] = int.Parse(textBoxMonth.Text);
                date[dateYearIndex] = int.Parse(textBoxYear.Text);
                return date;
            }
            internal static int GetYearIndex()
            {
                return dateYearIndex;
            }
            internal static int GetMonthIndex()
            {
                return dateMonthIndex;
            }
            internal static int GetDayIndex()
            {
                return dateDayIndex;
            }
        }
        internal class BirthDate : Date
        {
            private const string labelText = "date of birth (day-month-year):";
            private const string buttonText = "remove";

            private Button button;

            internal BirthDate(Form1 form) : base(form, labelText)
            {
                label.Text = labelText;

                button = new Button();
                button.Text = buttonText;
                button.Click += new EventHandler(ButtonFunction);
                form.Controls.Add(button);
            }
            private void ButtonFunction(object sender, EventArgs e)
            {
                form.Controls.Remove(label);
                form.Controls.Remove(textBoxDay);
                form.Controls.Remove(textBoxMonth);
                form.Controls.Remove(textBoxYear);
                form.Controls.Remove(button);
                form.BirthDatesRemove(this);
                form.ResetPositions();
            }
            internal void ChangePosition(int widthOfset, int heightOfset)
            {
                CorrectPosition(widthOfset, heightOfset);
                button.Location = new Point(widthOfset + textBoxOfset + textBoxBetweenOfset * 3, heightOfset);
            }
        }
    }
}
