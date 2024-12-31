using GraduationProjectIdeasPlatform.Data.Models;
using Ideaa.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ideaa.Comment
{
    public class Comment_Relationships
    {
        public void ConfigureIdeaSupervisorRelationshipKey()
        {
            // إعداد المفتاح المركب لجداول العلاقات الوسيطة
            // هذا الكود يخص كيان IdeaSupervisor الذي يمثل العلاقة بين جدولين:
            // جدول الأفكار (Idea) وجدول المستخدمين (User).

            // الهدف من المفتاح المركب هو التأكد من أن كل علاقة بين فكرة معينة (IdeaId) ومشرف معين (UserId)
            // تكون فريدة، أي لا يمكن إدخال نفس العلاقة مرتين في جدول IdeaSupervisor.

         
        }

        public void ConfigureIdeaStudentRelationshipKey()
        {
            // العلاقة بين Idea و User عبر جدول وسيط IdeaStudent
            // جدول IdeaStudent يحتوي على مفتاحين: IdeaId و UserId
            // هذا المفتاح المركب يضمن أن كل طالب مرتبط بفكرة واحدة فقط بشكل فريد

            /*
              modelBuilder.Entity<Notification>()
                    .HasOne(n => n.User)                    // إشعار واحد مرتبط بمستخدم واحد
                    .WithMany(u => u.Notifications)         // المستخدم يمكن أن يمتلك عدة إشعارات
                    .HasForeignKey(n => n.UserId)           // مفتاح الربط هو UserId في Notification
                    .OnDelete(DeleteBehavior.Cascade);      // عند حذف المستخدم، تحذف إشعاراته

              */

        }

        public void ConfigureNotificationIdeaRelationship()
        {
            // العلاقة بين Notification و Idea
            // جدول Notification يحتوي على مفتاح IdeaId لربط الإشعار بفكرة معينة
            // سبب العلاقة: كل إشعار يمكن أن يكون مرتبطًا بفكرة (Idea)

            /*
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Idea)                    // إشعار واحد مرتبط بفكرة واحدة
                .WithMany(i => i.Notifications)         // الفكرة يمكن أن تحتوي على عدة إشعارات
                .HasForeignKey(n => n.IdeaId)           // مفتاح الربط هو IdeaId في Notification
                .OnDelete(DeleteBehavior.Cascade);      // عند حذف الفكرة، تحذف إشعاراتها
            */
        }

        public void ConfigureIdeaSupervisorInverseRelationship()
        {
            // العلاقة بين IdeaSupervisor و Idea (ربط عكسي)
            // جدول IdeaSupervisor يربط بين Idea و User (المشرف)
            // سبب العلاقة: كل مشرف (User) يجب أن يكون مرتبطًا بفكرة واحدة أو أكثر

            /*
            modelBuilder.Entity<IdeaSupervisor>()
                .HasOne(n => n.Idea)                    // كل سجل في IdeaSupervisor مرتبط بفكرة واحدة
                .WithMany(i => i.IdeaSupervisors)       // الفكرة يمكن أن يكون لها عدة مشرفين
                .HasForeignKey(n => n.IdeaId);          // مفتاح الربط هو IdeaId
            */
        }

        public void ConfigureIdeaSupervisorToUserRelationship()
        {
            // العلاقة بين IdeaSupervisor و User
            // جدول IdeaSupervisor يربط بين Idea و User (المشرف)
            // سبب العلاقة: كل مشرف (User) موجود في جدول IdeaSupervisor

            /*
            modelBuilder.Entity<IdeaSupervisor>()
                .HasOne(n => n.User)                    // كل سجل في IdeaSupervisor مرتبط بمستخدم واحد
                .WithMany()                             // لا يوجد ربط عكسي في User
                .HasForeignKey(n => n.UserId);          // مفتاح الربط هو UserId
            */
        }

        public void ConfigureIdeaStudentInverseRelationship()
        {
            // العلاقة بين IdeaStudent و Idea (ربط عكسي)
            // جدول IdeaStudent يربط بين Idea و User (الطالب)
            // سبب العلاقة: كل طالب (User) يجب أن يكون مرتبطًا بفكرة واحدة أو أكثر

            /*
            modelBuilder.Entity<IdeaStudent>()
                .HasOne(n => n.Idea)                    // كل سجل في IdeaStudent مرتبط بفكرة واحدة
                .WithMany(i => i.IdeaStudents)          // الفكرة يمكن أن يكون لها عدة طلاب
                .HasForeignKey(n => n.IdeaId);          // مفتاح الربط هو IdeaId
            */
        }


        public void ConfigureIdeaStudentToUserRelationship()
        {
            // العلاقة بين IdeaStudent و User
            // جدول IdeaStudent يربط بين Idea و User (الطالب)
            // سبب العلاقة: كل طالب (User) موجود في جدول IdeaStudent

            /*
            modelBuilder.Entity<IdeaStudent>()
                .HasOne(n => n.User)                    // كل سجل في IdeaStudent مرتبط بمستخدم واحد
                .WithMany()                             // لا يوجد ربط عكسي في User
                .HasForeignKey(n => n.UserId);          // مفتاح الربط هو UserId
            */
        }



    }
}
