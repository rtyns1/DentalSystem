using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // use this when 

namespace DentalSystem.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
       // sqaure brackets are attributes, attributes add metadata basically extra info to code elements like classes, properties, methods
                                   // metadata is used by Entity framework, ASP.NET validation, etc at runtime or design time. They are like labels, or stickers that attach to code and tell the framework how to handleit.
        /*
         * [Key] -- PRIMARY KEY FOR ENTITY FRAMEWORK
         * - Entity frameowk allows users to write SQL code entirely inside C#, entirely eliminating the need to write tedious and repetitive SQL quesries for data access.
         * - so EF is very important if your project uses SQL and is data intensive.
         * - so, [Key] is an attribute that tells EF that this proeprty is the PRIMARY KEY of the database.
         * - By default, EF loooks for property named Id or ClassNameId as the primary key so if u intend soemthing esle to be a primarykey, you must use the attirbute [Key]
         * - 
         */
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;
        /*
         * The Required attribute is a validation and databse constraint----- EF creates NOT NULL in the column.
         * A thr application level, ASP.NET core uses it for model validation - when submiting a form.
         *  Here it ensures that FullName must have a value before saving to the database or accepting user input esle if its null or empty string == exeception thrown
         *  Display attribute is a UI lable -- spsecifies the frinedlyer name to be used in UI forms or error messages, but it is not used by EF for DB mapping.
         *  ASP.NET core uses it when generating k=lables.
         *  UI will show Full Name instead of FullName.
         *  So all thes are used extensively
         */

        public virtual ICollection<TreatmentPlan> TreatmentPlans { get; set; } = new List<TreatmentPlan>();
        /*
         * This is the first time using ICollcetion and in Karel's fashion -- a long coment block explainig everything.
         * Okay, this is quite a long topic because ti involves some data structures thinking but i short, an Array is fixed, so not good for tracking changes.
         * In short words, we use ICollection rather than a conrete List primarily to write flexible code that relies on abstractions rather than strict unchangeable data types.
         * Essentially, ICollection is a set of rules, an interface whilst a List is a sepcific product that follows those rules, so they are not even close to be the same thing.
         * Lists and Arrays are useful and have their own usecases, but for EF framework, ORMs(object relationa mappers) prefer ICollection for database relationships. This allows the framework to sneak in its own special lazy loading collections
         * instead of being forced to load havy, static memory Lists riht away.
         * So, understna
         */
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

    }
}
