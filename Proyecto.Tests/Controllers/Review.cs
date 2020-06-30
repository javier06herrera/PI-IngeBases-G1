using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Tests.Controllers
{
    class Review
    {
        private readonly IReview _review;
        public Review(IReview review)
        {
            if(review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }
            _review = review;
        }

        public int articleId() => _review.articleId;
        public string email() => _review.email;
        public string comments() => _review.comments;
        public int generalOpinion() => _review.generalOpinion;
        public int communityContribution() => _review.communityContribution;
        public int articleStructure() => _review.articleStructure;
        public int totalGrade => _review.totalGrade;
        public string state => _review.state;
    }
}
