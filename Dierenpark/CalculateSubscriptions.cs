using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Dierenpark
{
    class CalculateSubscriptions
    {
        private const int adultArrayPosition = 0;
        private const int childArrayPosition = 1;
        private const int elderlyArrayPosition = 2;

        private const int arraySize = 3;

        private const int minAdultAge = 18;
        private const int minElderlyAge = 65;

        private const string nameCouple = "Married couple";
        private const string nameFamilyOneChild = "Gezin met 1 kind";
        private const string nameFamilyTwoChilderen = "Gezin met 2 kinderen";
        private const string namePerson = "Person";
        private const string nameExtraChild = "an extra child";
        private const string nameElderlyCouple = "Couple 65+";
        private const string nameElderlyPerson = "Person 65+";

        private static readonly int[] qualifierCouple = { 2, 0, 0 };
        private static readonly int[] qualifierFamilyOneChild = { 2, 1, 0 };
        private static readonly int[] qualifierFamilyTwoChilderen = { 2, 2, 0 };
        private static readonly int[] qualifierPerson = { 1, 0, 0 };
        private static readonly int[] qualifierExtraChild = { 0, 1, 0 };
        private static readonly int[] qualifierElderlyCouple = { 0, 0, 2 };
        private static readonly int[] qualifierElderlyPerson = { 0, 0, 1 };

        private const int priceCouple = 61;
        private const int priceFamilyOneChild = 71;
        private const int priceFamilyTwoChilderen = 82;
        private const int pricePerson = 30;
        private const int priceExtraChild = 11;
        private const int priceElderlyCouple = 65;
        private const int priceElderlyPerson = 26;

        private const string stringBreak = "\r\n";

        private static List<SubscriptionOption> subscriptionOptions;

        public static int GetArraySize()
        {
            return arraySize;
        }
        public static int GetAdultArrayPosition()
        {
            return adultArrayPosition;
        }
        public static int GetChildArrayPosition()
        {
            return childArrayPosition;
        }
        public static int GetElderlyArrayPosition()
        {
            return elderlyArrayPosition;
        }
        public static int GetMinAdultAge()
        {
            return minAdultAge;
        }
        public static int GetMinElderlyAge()
        {
            return minElderlyAge;
        }

        public static string GetSubscriptionsString(int[] peopleCount)
        {
            SubscriptionOptionsExists();
            List<SubscriptionOption> subscriptions = GetSubscriptionOptionsOfPeopleCount(peopleCount, new List<SubscriptionOption>(), null);
            string toReturn = "";
            foreach(SubscriptionOption subscription in subscriptions)
            {
                toReturn += subscription.GetName() + stringBreak;
            }
            toReturn += GetSubscriptionOptionsTotalPrice(subscriptions);
            return toReturn;
        }
        private static List<SubscriptionOption> GetSubscriptionOptionsOfPeopleCount (int[] peopleCount, List<SubscriptionOption> workingOnOptions, List<SubscriptionOption> workingOptions)
        {
            foreach(SubscriptionOption subscriptionOption in subscriptionOptions)
            {
                if (subscriptionOption.CanApply(peopleCount))
                {
                    List<SubscriptionOption> newSubscriptionOptions = CopySubscriptionOptions(workingOnOptions);
                    newSubscriptionOptions.Add(subscriptionOption);

                    if (workingOptions == null || GetSubscriptionOptionsTotalPrice(newSubscriptionOptions) < GetSubscriptionOptionsTotalPrice(workingOptions))
                    {

                        int[] newPeopleCount = subscriptionOption.Apply(peopleCount);

                        bool noPeopleLeft = true;
                        foreach (int count in newPeopleCount)
                        {
                            if (count != 0)
                            {
                                noPeopleLeft = false;
                                break;
                            }
                        }
                        if (noPeopleLeft)
                        {
                            workingOptions = newSubscriptionOptions;
                        }
                        else
                        {
                            workingOptions = GetSubscriptionOptionsOfPeopleCount(newPeopleCount, newSubscriptionOptions, workingOptions);
                        }
                    }
                }
            }
            return workingOptions;
        }

        private static int GetSubscriptionOptionsTotalPrice(List<SubscriptionOption> subscriptionOptions)
        {
            int total = 0;
            foreach(SubscriptionOption option in subscriptionOptions)
            {
                total += option.GetPrice();
            }
            return total;
        }
        private static List<SubscriptionOption> CopySubscriptionOptions(List<SubscriptionOption> toCopy)
        {
            List<SubscriptionOption> toReturn = new List<SubscriptionOption>();
            foreach(SubscriptionOption option in toCopy)
            {
                toReturn.Add(option);
            }
            return toReturn;
        }

        private static void SubscriptionOptionsExists()
        {
            if(subscriptionOptions == null)
            {
                subscriptionOptions = new List<SubscriptionOption>();

                subscriptionOptions.Add(new SubscriptionOption(nameCouple, qualifierCouple, priceCouple));
                subscriptionOptions.Add(new SubscriptionOption(nameFamilyOneChild, qualifierFamilyOneChild, priceFamilyOneChild));
                subscriptionOptions.Add(new SubscriptionOption(nameFamilyTwoChilderen, qualifierFamilyTwoChilderen, priceFamilyTwoChilderen));
                subscriptionOptions.Add(new SubscriptionOption(namePerson, qualifierPerson, pricePerson));
                subscriptionOptions.Add(new SubscriptionOption(nameExtraChild, qualifierExtraChild, priceExtraChild));
                subscriptionOptions.Add(new SubscriptionOption(nameElderlyCouple, qualifierElderlyCouple, priceElderlyCouple));
                subscriptionOptions.Add(new SubscriptionOption(nameElderlyPerson, qualifierElderlyPerson, priceElderlyPerson));
            }
        }

        private class SubscriptionOption
        {
            private const string notSameLenthExeptionText = "array given to SubscriptionOption isn't the same size as it's qualifier";

            private string name;
            private int[] qualifier;
            private int price;

            internal SubscriptionOption(string name, int[] qualifier, int price)
            {
                this.name = name;
                this.qualifier = qualifier;
                this.price = price;
            }
            internal bool CanApply(int[] peopleCount)
            {
                if (peopleCount.Length == qualifier.Length)
                {
                    for (int i = 0; i < qualifier.Length; i++)
                    {
                        if (peopleCount[i] < qualifier[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
                throw new Exception(notSameLenthExeptionText);
            }
            internal int[] Apply(int[] peopleCount)
            {
                if (peopleCount.Length == qualifier.Length)
                {
                    int[] toReturn = new int[qualifier.Length];
                    for (int i = 0; i < qualifier.Length; i++)
                    {
                        toReturn[i] = peopleCount[i] - qualifier[i];
                    }
                    return toReturn;
                }
                throw new Exception(notSameLenthExeptionText);
            }
            internal string GetName()
            {
                return name;
            }
            internal int GetPrice()
            {
                return price;
            }
        }
    }
}
