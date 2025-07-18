from flask import render_template, request,jsonify
from . import recommendation_bp
from .utils import AcademicAdvisor
from flask import Flask
from flask_cors import CORS

# app = Flask(__name__)
# CORS(app, origins="*")

@recommendation_bp.route('/', methods=['GET', 'POST'])
def recommend():
    if request.method == 'POST':
        student_id = request.form['student_id']
        gpa = float(request.form['gpa'])
        expected_to_graduate = request.form['expected_to_graduate'] == 'True'
        semester = request.form['semester']
        term = request.form['term']
        department = request.form['department']

        advisor = AcademicAdvisor(student_id, gpa, expected_to_graduate, semester, term, department)
        result = advisor.run()
        return render_template('recommendation.html', result=result)

    return render_template('recommendation.html')

@recommendation_bp.route('/api/recommend', methods=['POST'])
def api_recommend():
    try:
        data = request.get_json()

        # 🔻 Extract required fields from backend
        print("Term: ", data['Term'])
        print("Compeleted Courses: ", data['CompletedCourses'])

        student_id = data['StudentId']
        gpa = data['GPA']
        expected_to_graduate = data.get('expected_to_graduate', False)
        semester = data.get('semester',"regular")
        term = data['Term']
        department = data['DepartmentName']
        completed_course_details = data['CompletedCourses']
        all_courses = data['AllCourses']

        # ✅ Override the fetch functions by injecting the data directly
        advisor = AcademicAdvisor(
            student_id,
            gpa,
            expected_to_graduate,
            semester,
            term,
            department,
            completed_course_details=completed_course_details,
            all_courses=all_courses
        )

        result = advisor.run()
        print("result: ", result)
        return jsonify(result), 200

    except Exception as e:
        return jsonify({"error": str(e)}), 500