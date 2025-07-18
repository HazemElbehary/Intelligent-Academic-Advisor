from collections import defaultdict
import re

# Completed courses (IDs)
def fetch_completed_courses(student_id):
    return  ["HU111","MA111","MA112","IT111","CS111","MA113","CS112","HU124","HU118","HU112","IT212","IT221","ST121","MA214","HU225","CS213","CS251"
    ,"ST222","DS211","CS214", "IS211", "IS231", "CS321","CS341","AI311","AI321","CS331","IT341","AI312","AI313", "AI322", "AI331","AI332",
    ]

def fetch_completed_course_details(studnet_id):
    return [
{"code": "HU111","course_name": "Technical Report Writing","credit_hours": 2,"distribution_category": "General_Requirements","type": "Mandatory","prerequisites": [],"Term": "First","department": "null"},
{"code": "MA111","course_name": "Math-1","credit_hours": 3,"distribution_category": "Math_And_Basic_Sciences","type": "Mandatory","prerequisites": [],"Term": "All","department": "null"},
{"code": "MA112","course_name": "Discrete Mathematics","credit_hours": 3,"distribution_category": "Math_And_Basic_Sciences","type": "Mandatory","prerequisites": [],"Term": "First","department": "null"},
{"code": "IT111","course_name": "Electronics","credit_hours": 3,"distribution_category": "Math_And_Basic_Sciences","type": "Mandatory","prerequisites": [],"Term": "All","department": "null"},
{"code": "CS111","course_name": "Fundamentals of Computer Science","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": [],"Term": "All","department": "null"},

{"code": "MA113","course_name": "Math-2","credit_hours": 3,"distribution_category": "Math_And_Basic_Sciences","type": "Mandatory","prerequisites": ["MA111"],"Term": "All","department": "null"},
{"code": "CS112","course_name": "Structured Programming","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS111"],"Term": "All","department": "null"},
{"code": "HU124","course_name": "Critical Thinking","credit_hours": 2,"distribution_category": "General_Requirements","type": "Mandatory","prerequisites": [],"Term": "Second","department": "null"},
{"code": "HU118","course_name": "Selected Topics in Humanities","credit_hours": 2,"distribution_category": "General_Requirements","type": "Elective","prerequisites": [],"Term": "Second","department": "null"},
{"code": "HU112","course_name": "Ethics and Professionalism","credit_hours": 2,"distribution_category": "General_Requirements","type": "Mandatory","prerequisites": [],"Term": "Second","department": "null"},
{"code": "IT212","course_name": "Logic Design","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory", "prerequisites": ["IT111"],"Term": "All","department": "null"},
{"code": "IT221","course_name": "Computer Networks Technology","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS111"],"Term": "All","department": "null"},

{"code": "ST121","course_name": "Probability and Statistics-1","credit_hours": 3,"distribution_category": "Math_And_Basic_Sciences","type": "Mandatory","prerequisites": ["MA111"],"Term": "All","department": "null"},
{"code": "MA214","course_name": "Math-3","credit_hours": 3,"distribution_category": "Math_And_Basic_Sciences","type": "Mandatory","prerequisites": ["MA113"],"Term": "All","department": "null"},
{"code": "HU225","course_name": "Entrepreneurship","credit_hours": 2,"distribution_category": "General_Requirements","type": "Elective","prerequisites": ["HU124"],"Term": "First","department": "null"},
{"code": "CS213","course_name": "Object Oriented Programming","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS112"],"Term": "All","department": "null"},
{"code": "CS251","course_name": "Introduction to Software Engineering","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS112"],"Term": "All","department": "null"},

{"code": "ST222","course_name": "Probability and Statistics-2","credit_hours": 3,"distribution_category": "Math_And_Basic_Sciences","type": "Mandatory","prerequisites": ["ST121"],"Term": "All","department": "null"},
{"code": "DS211","course_name": "Introduction to Operations Research and Decision Support","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS112","ST121"],"Term": "All","department": "null"},
{"code": "CS214","course_name": "Data Structures","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS213"],"Term": "All","department": "null"},
{"code": "IS211","course_name": "Introduction to Database Systems","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS112"],"Term": "Second","department": "null"},
{"code": "IS231","course_name": "Web Technology","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS213"],"Term": "All","department": "null"},

{"code": "CS321","course_name": "Algorithms Analysis and Design","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS214"],"Term": "First","department": "null"},
{"code": "CS341","course_name": "Operating Systems","credit_hours": 3,"distribution_category": "Basic_Computer_Science","type": "Mandatory","prerequisites": ["CS214"],"Term": "First","department": "null"},
{"code": "AI311","course_name": "Introduction to Logic","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["MA112","ST222"],"Term": "First","department": "AI"},
{"code": "AI321","course_name": "Theoretical Foundations of Machine Learning","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["MA214","ST222"],"Term": "All","department": "AI"},
{"code": "CS331","course_name": "Computer Organization and Architecture","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["IT212","CS214"],"Term": "First","department": "AI"},
{"code": "IT341","course_name": "Signals and Systems","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["MA214"],"Term": "First","department": "AI"},

{"code": "AI312","course_name": "Reasoning and Knowledge Representation","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["AI311"],"Term": "Second","department": "AI"},
{"code": "AI313","course_name": "Autonomous Multiagent Systems","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["AI311"],"Term": "Second","department": "AI"},
{"code": "AI322","course_name": "Supervised Learning","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["AI321"],"Term": "Second","department": "AI"},
{"code": "AI331","course_name": "Theories of Mind","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["MA112"],"Term": "Second","department": "AI"},
{"code": "AI332","course_name": "Computational Cognitive Science","credit_hours": 3,"distribution_category": "Applied_Sciences","type": "Mandatory","prerequisites": ["AI311"],"Term": "Second","department": "AI"},
]

def fetch_all_courses():
    return [

    {
        "code": "HU111",
        "course_name": "Technical Report Writing",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Mandatory",
        "prerequisites": [],
        "Term": "First",
        "department": "null"
    },
    {
        "code": "MA111",
        "course_name": "Math-1",
        "credit_hours": 3,
        "distribution_category": "Math_And_Basic_Sciences",
        "type": "Mandatory",
        "prerequisites": [],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "MA112",
        "course_name": "Discrete Mathematics",
        "credit_hours": 3,
        "distribution_category": "Math_And_Basic_Sciences",
        "type": "Mandatory",
        "prerequisites": [],
        "Term": "First",
        "department": "null"
    },
    {
        "code": "IT111",
        "course_name": "Electronics",
        "credit_hours": 3,
        "distribution_category": "Math_And_Basic_Sciences",
        "type": "Mandatory",
        "prerequisites": [],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "CS111",
        "course_name": "Fundamentals of Computer Science",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "HU113",
        "course_name": "Creative Thinking and Communication Skills",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [],
        "Term": "First",
        "department": "null"
    },
    {
        "code": "MA113",
        "course_name": "Math-2",
        "credit_hours": 3,
        "distribution_category": "Math_And_Basic_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA111"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "ST121",
        "course_name": "Probability and Statistics-1",
        "credit_hours": 3,
        "distribution_category": "Math_And_Basic_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA111"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "CS112",
        "course_name": "Structured Programming",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS111"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "HU124",
        "course_name": "Critical Thinking",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Mandatory",
        "prerequisites": [],
        "Term": "Second",
        "department": "null"
    },
    {
        "code": "HU118",
        "course_name": "Selected Topics in Humanities",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [],
        "Term": "Second",
        "department": "null"
    },
    {
        "code": "HU121",
        "course_name": "Fundamentals of Economics",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [],
        "Term": "Second",
        "department": "null"
    },
    {
        "code": "HU112",
        "course_name": "Ethics and Professionalism",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Mandatory",
        "prerequisites": [],
        "Term": "Second",
        "department": "null"
    },
    {
        "code": "DS251",
        "course_name": "Fundamentals of Management",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [
            "Passing 30 Credit Hours"
        ],
        "Term": "Second",
        "department": "null"
    },
    {
        "code": "HU123",
        "course_name": "Marketing and Sales",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [],
        "Term": "null",
        "department": "null"
    },
    {
        "code": "HU114",
        "course_name": "Fundamentals of Psychology",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [],
        "Term": "null",
        "department": "null"
    },
    {
        "code": "HU115",
        "course_name": "Fundamentals of Sociology",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [],
        "Term": "null",
        "department": "null"
    },
    {
        "code": "HU116",
        "course_name": "Comparative Politics",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [],
        "Term": "null",
        "department": "null"
    },
    {
        "code": "HU117",
        "course_name": "Human Rights",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [],
        "Term": "null",
        "department": "null"
    },
    {
        "code": "HU225",
        "course_name": "Entrepreneurship",
        "credit_hours": 2,
        "distribution_category": "General_Requirements",
        "type": "Elective",
        "prerequisites": [
            "HU124"
        ],
        "Term": "First",
        "department": "null"
    },
    {
        "code": "MA214",
        "course_name": "Math-3",
        "credit_hours": 3,
        "distribution_category": "Math_And_Basic_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA113"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "ST222",
        "course_name": "Probability and Statistics-2",
        "credit_hours": 3,
        "distribution_category": "Math_And_Basic_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "ST121"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "CS213",
        "course_name": "Object Oriented Programming",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS112"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "CS214",
        "course_name": "Data Structures",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS213"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "CS251",
        "course_name": "Introduction to Software Engineering",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS112"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "DS211",
        "course_name": "Introduction to Operations Research and Decision Support",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS112",
            "ST121"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "IS211",
        "course_name": "Introduction to Database Systems",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS112"
        ],
        "Term": "Second",
        "department": "null"
    },
    {
        "code": "IS231",
        "course_name": "Web Technology",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS213"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "IT212",
        "course_name": "Logic Design",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "IT111"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "IT221",
        "course_name": "Computer Networks Technology",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS111"
        ],
        "Term": "All",
        "department": "null"
    },
    {
        "code": "CS321",
        "course_name": "Algorithms Analysis and Design",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS214"
        ],
        "Term": "First",
        "department": "null"
    },
    {
        "code": "CS341",
        "course_name": "Operating Systems",
        "credit_hours": 3,
        "distribution_category": "Basic_Computer_Science",
        "type": "Mandatory",
        "prerequisites": [
            "CS214"
        ],
        "Term": "First",
        "department": "null"
    },
    {
        "code": "CS316",
        "course_name": "Advanced Data Structures",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS214"
        ],
        "Term": "First",
        "department": "CS"
    },
    {
        "code": "IT351",
        "course_name": "Information Theory and Data Compression",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA214",
            "CS213"
        ],
        "Term": "First",
        "department": "CS"
    },
    {
        "code": "CS352",
        "course_name": "Advanced Software Engineering",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS251"
        ],
        "Term": "First",
        "department": "CS"
    },
    {
        "code": "CS331",
        "course_name": "Computer Organization and Architecture",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS214",
            "IT212"
        ],
        "Term": "First",
        "department": "CS"
    },
    {
        "code": "CS322",
        "course_name": "Concepts of Programming Languages",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS316"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "CS342",
        "course_name": "Advanced Operating Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS341"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "CS361",
        "course_name": "Artificial Intelligence",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS214"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "CS371",
        "course_name": "High Performance Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS341"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "IT361",
        "course_name": "Computer Graphics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS112"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "CS423",
        "course_name": "Compilers",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS322"
        ],
        "Term": "First",
        "department": "CS"
    },
    {
        "code": "CS462",
        "course_name": "Machine Learning",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "ST222",
            "MA214",
            "CS213"
        ],
        "Term": "First",
        "department": "CS"
    },
    {
        "code": "CS432",
        "course_name": "Computation Theory",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA112",
            "MA214"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "CS472",
        "course_name": "Cloud Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS342"
        ],
        "Term": "First",
        "department": "CS"
    },
    {
        "code": "CS434",
        "course_name": "Big Data Analysis",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS211",
            "CS462"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS435",
        "course_name": "Bioinformatics Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS321"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS436",
        "course_name": "Mobile Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS341"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS453",
        "course_name": "Software Testing and Quality Assurance",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS321",
            "CS352"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "CS454",
        "course_name": "Software Security",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS321",
            "CS352"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS455",
        "course_name": "Human Computer Interaction",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS321",
            "CS352"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS456",
        "course_name": "Software Design and Architecture",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS321",
            "CS352"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS457",
        "course_name": "Selected Topics in Software Engineering",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS352"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS463",
        "course_name": "Natural Language Processing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS462"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "CS464",
        "course_name": "Semantic Web and Ontology",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS361",
            "IS231"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS465",
        "course_name": "Soft Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "ST121",
            "MA113"
        ],
        "Term": "First",
        "department": "CS"
    },
    {
        "code": "CS466",
        "course_name": "Knowledge Discovery",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS211",
            "CS361"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS467",
        "course_name": "Selected Topics in Artificial Intelligence",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS462"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS473",
        "course_name": "Advanced High Performance Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS371",
            "CS342"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS474",
        "course_name": "Selected Topics in High Performance Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS473"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS495",
        "course_name": "Selected Topics in Computer Science–1",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS322"
        ],
        "Term": "Second",
        "department": "CS"
    },
    {
        "code": "CS496",
        "course_name": "Selected Topics in Computer Science–2",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS322"
        ],
        "Term": "null",
        "department": "CS"
    },
    {
        "code": "CS497",
        "course_name": "Graduation_Project-1",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours"
        ],
        "Term": "All",
        "department": "CS"
    },
    {
        "code": "CS498",
        "course_name": "Graduation_Project-2",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours",
            "CS497"
        ],
        "Term": "All",
        "department": "CS"
    },
    {
        "code": "IT313",
        "course_name": "Computer Architecture",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT212"
        ],
        "Term": "First",
        "department": "IT"
    },
    {
        "code": "IT314",
        "course_name": "Micro Controllers",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT212"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT322",
        "course_name": "Advanced Computer Networks",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT221",
            "IT331"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT331",
        "course_name": "Data Communication",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA113"
        ],
        "Term": "All",
        "department": "IT"
    },
    {
        "code": "IT341",
        "course_name": "Signals and Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA214"
        ],
        "Term": "First",
        "department": "IT"
    },
    {
        "code": "IT342",
        "course_name": "Digital Signal Processing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT341"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT351",
        "course_name": "Information Theory and Data Compression",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA214",
            "CS213"
        ],
        "Term": "First",
        "department": "IT"
    },
    {
        "code": "IT352",
        "course_name": "Pattern Recognition",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "ST222",
            "IT341"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT361",
        "course_name": "Computer Graphics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS112"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT423",
        "course_name": "Information and Computer Networks Security",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT322"
        ],
        "Term": "First",
        "department": "IT"
    },
    {
        "code": "IT432",
        "course_name": "Communication Technology",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT221"
        ],
        "Term": "First",
        "department": "IT"
    },
    {
        "code": "IT443",
        "course_name": "Image Processing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT341"
        ],
        "Term": "All",
        "department": "IT"
    },
    {
        "code": "IT444",
        "course_name": "Multimedia Mining",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT352"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT415",
        "course_name": "Machine Vision",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT443",
            "IT361"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT416",
        "course_name": "Robotics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT314"
        ],
        "Term": "First",
        "department": "IT"
    },
    {
        "code": "IT417",
        "course_name": "Embedded Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT314"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT424",
        "course_name": "Wireless and Mobile Networks",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT322"
        ],
        "Term": "First",
        "department": "IT"
    },
    {
        "code": "IT425",
        "course_name": "Cloud Computing Networks",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT322"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT426",
        "course_name": "Internet Programming and Protocols",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT322"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT427",
        "course_name": "Optical Networks",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT322"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT428",
        "course_name": "Wireless Sensor Networks",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT424"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT429",
        "course_name": "Selected Topics in Computer Networks",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT322"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT433",
        "course_name": "Cyber Security",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT423"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT445",
        "course_name": "Advanced Image Processing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT443"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT446",
        "course_name": "Virtual Reality",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT443",
            "IT361"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT447",
        "course_name": "Speech Processing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT342"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT448",
        "course_name": "Selected Topics in Multimedia",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT352"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT453",
        "course_name": "Advanced Pattern Recognition",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT352"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT454",
        "course_name": "Human Language Technology",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT352"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT462",
        "course_name": "Advanced Computer Graphics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT361"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT463",
        "course_name": "Computer Animation",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT361"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT471",
        "course_name": "Ubiquitous Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT322",
            "CS112"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT472",
        "course_name": "Concurrency and Parallel Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT313",
            "CS112"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT473",
        "course_name": "Intelligent and Quantum Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT313",
            "CS214"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT495",
        "course_name": "Selected Topics in Information Technology-1",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "Passing 60 Credit Hours"
        ],
        "Term": "Second",
        "department": "IT"
    },
    {
        "code": "IT496",
        "course_name": "Selected Topics in Information Technology-2",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "Passing 60 Credit Hours"
        ],
        "Term": "null",
        "department": "IT"
    },
    {
        "code": "IT497",
        "course_name": "Graduation_Project-1",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours"
        ],
        "Term": "All",
        "department": "IT"
    },
    {
        "code": "IT498",
        "course_name": "Graduation_Project-2",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours",
            "IT497"
        ],
        "Term": "All",
        "department": "IT"
    },
    {
        "code": "IS312",
        "course_name": "Database Management Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            " (IS211)",
            " (CS213)"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "IS313",
        "course_name": "Data Warehousing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IS211"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "IS321",
        "course_name": "File Management and Processing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS214"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "IS322",
        "course_name": "Information Retrieval",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IS211",
            "ST222"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "IS332",
        "course_name": "Analysis and Design of Information Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IS211"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "IS333",
        "course_name": "Web-based Information Systems Development",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IS231"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "IS341",
        "course_name": "Business Process Management",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IS332"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "CS352",
        "course_name": "Advanced Software Engineering",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS251"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "CS361",
        "course_name": "Artificial Intelligence",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS214"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "IS414",
        "course_name": "Managing and Modeling Big Data",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IS312"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "IS422",
        "course_name": "Data Mining",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IS322"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "IS434",
        "course_name": "Service-Oriented Architecture",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IS333"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "CS462",
        "course_name": "Machine Learning",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "ST121",
            "MA214",
            "CS213"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "IS331",
        "course_name": "Fundamentals of Information Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS211"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS415",
        "course_name": "Cloud Database",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS312"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS416",
        "course_name": "Distributed Database",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS312"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS417",
        "course_name": "Selected Topics in Databases",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS312"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS423",
        "course_name": "Business Process Mining",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS341"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "IS424",
        "course_name": "Selected Topics in Data Engineering",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS312"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS435",
        "course_name": "Usability Engineering",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS231"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS436",
        "course_name": "Enterprise Mobile Applications Development",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS231"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "IS437",
        "course_name": "Information Systems Development Methodologies",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "IS438",
        "course_name": "Management Information Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS439",
        "course_name": "Selected Topics in Advanced Information Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS442",
        "course_name": "Geographical Information Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS312"
        ],
        "Term": "First",
        "department": "IS"
    },
    {
        "code": "IS443",
        "course_name": "Information Systems Quality Assurance",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS444",
        "course_name": "Information Systems Security and Risk Management",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS445",
        "course_name": "Information Systems Audit and Control",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS446",
        "course_name": "Enterprise Information Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS333"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS447",
        "course_name": "Information Systems Project Management",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS448",
        "course_name": "E-Business",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "Second",
        "department": "IS"
    },
    {
        "code": "IS449",
        "course_name": "Selected Topics in Information Systems Engineering",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS333"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS495",
        "course_name": "Selected Topics in Information Systems-1",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS496",
        "course_name": "Selected Topics in Information Systems-2",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS332"
        ],
        "Term": "null",
        "department": "IS"
    },
    {
        "code": "IS497",
        "course_name": "Graduation_Project-1",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours"
        ],
        "Term": "All",
        "department": "IS"
    },
    {
        "code": "IS498",
        "course_name": "Graduation_Project-2",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours",
            "IS497"
        ],
        "Term": "All",
        "department": "IS"
    },
    {
        "code": "AI311",
        "course_name": "Introduction to Logic",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA112",
            "ST222"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "AI312",
        "course_name": "Reasoning and Knowledge Representation",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "AI311"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "AI313",
        "course_name": "Autonomous Multiagent Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "AI311"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "AI321",
        "course_name": "Theoretical Foundations of Machine Learning",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA214",
            "ST222"
        ],
        "Term": "All",
        "department": "AI"
    },
    {
        "code": "AI322",
        "course_name": "Supervised Learning",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "AI321"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "AI331",
        "course_name": "Theories of Mind",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA112"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "AI332",
        "course_name": "Computational Cognitive Science",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "AI311"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "AI414",
        "course_name": "Processing of Formal and Natural Languages",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "AI311"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "AI423",
        "course_name": "Unsupervised Learning",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "AI321"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "AI424",
        "course_name": "Reinforcement Learning",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "AI322"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "AI441",
        "course_name": "Intelligent Autonomous Robotics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "AI322"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "CS331",
        "course_name": "Computer Organization and Architecture",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "IT212",
            "CS214"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "IT341",
        "course_name": "Signals and Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "MA214"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "AI433",
        "course_name": "Cognitive Robotics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "AI322",
            "AI441"
        ],
        "Term": "null",
        "department": "AI"
    },
    {
        "code": "AI442",
        "course_name": "Generative Adversarial Networks",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "AI322"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "AI443",
        "course_name": "Fundamentals of Biometric Identification",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "AI322"
        ],
        "Term": "null",
        "department": "AI"
    },
    {
        "code": "AI444",
        "course_name": "Brain-Computer Interfacing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "AI322"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "AI445",
        "course_name": "Principles of Quantum Artificial Intelligence",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "AI321"
        ],
        "Term": "null",
        "department": "AI"
    },
    {
        "code": "AI446",
        "course_name": "Business Intelligence: Strategies, Tools & Techniques",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "AI321"
        ],
        "Term": "null",
        "department": "AI"
    },
    {
        "code": "AI447",
        "course_name": "Artificial Intelligence for Cyber Security",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "AI322",
            "IT221"
        ],
        "Term": "null",
        "department": "AI"
    },
    {
        "code": "CS435",
        "course_name": "Bioinformatics Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS321"
        ],
        "Term": "null",
        "department": "AI"
    },
    {
        "code": "CS465",
        "course_name": "Soft Computing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "ST121",
            "MA113"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "DS342",
        "course_name": "Data Analytics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "ST222"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "DS456",
        "course_name": "Project Management",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS211"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "IS322",
        "course_name": "Information Retrieval",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS211",
            "ST222"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "IS435",
        "course_name": "Usability Engineering",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IS231"
        ],
        "Term": "null",
        "department": "AI"
    },
    {
        "code": "IT361",
        "course_name": "Computer Graphics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "CS112"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "IT415",
        "course_name": "Machine Vision",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT443",
            "IT361"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "IT443",
        "course_name": "Image Processing",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT341"
        ],
        "Term": "All",
        "department": "AI"
    },
    {
        "code": "IT446",
        "course_name": "Virtual Reality",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "IT443",
            "CIT361"
        ],
        "Term": "null",
        "department": "AI"
    },

    {
        "code": "AI495",
        "course_name": "Selected Topics in Artificial Intelligence-1",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "Passing 60 Credit Hours"
        ],
        "Term": "First",
        "department": "AI"
    },
    {
        "code": "AI496",
        "course_name": "Selected Topics in Artificial Intelligence-2",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "Passing 60 Credit Hours"
        ],
        "Term": "Second",
        "department": "AI"
    },
    {
        "code": "AI497",
        "course_name": "Graduation_Project-1",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours"
        ],
        "Term": "All",
        "department": "AI"
    },
    {
        "code": "AI498",
        "course_name": "Graduation_Project-2",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours",
            "AI497"
        ],
        "Term": "All",
        "department": "AI"
    },
    {
        "code": "DS312",
        "course_name": "Decision Support and Future Studies Methodologies",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "DS211"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS313",
        "course_name": "Computational Intelligence",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "DS321",
            "ST222"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS321",
        "course_name": "Linear and Integer Programming",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "DS211",
            "MA113"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS322",
        "course_name": "Non-linear Programming",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "DS321"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS323",
        "course_name": "Dynamic Programming and Stochastic Modeling",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "DS321"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS331",
        "course_name": "System Modeling and Simulation",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS213"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS341",
        "course_name": "Learning From Data",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "ST222"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS352",
        "course_name": "Production and Operations Management",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "DS312"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "CS361",
        "course_name": "Artificial Intelligence",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "CS214"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS414",
        "course_name": "Game Theory",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "ST222",
            "DS321"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS415",
        "course_name": "Decision Theory",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "ST222",
            "DS321"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS424",
        "course_name": "Multi-objective Programming",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "DS322"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS425",
        "course_name": "Network Modeling and Optimization",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Mandatory",
        "prerequisites": [
            "DS323"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS342",
        "course_name": "Data Analytics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "ST222"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS343",
        "course_name": "Probabilistic Reasoning",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "ST222"
        ],
        "Term": "null",
        "department": "DS"
    },
    {
        "code": "DS344",
        "course_name": "Forecasting and Predictive Analytics",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS341"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS416",
        "course_name": "Strategic Decision Making",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS321"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS432",
        "course_name": "System Dynamics Modeling",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS331"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS433",
        "course_name": "Agent-Based Modeling and Complex Systems",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS331"
        ],
        "Term": "null",
        "department": "DS"
    },
    {
        "code": "DS453",
        "course_name": "Crisis Management",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS312"
        ],
        "Term": "null",
        "department": "DS"
    },
    {
        "code": "DS454",
        "course_name": "Service Management",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS312"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS455",
        "course_name": "Managerial Economics and Financial Analysis",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS312"
        ],
        "Term": "Second",
        "department": "DS"
    },
    {
        "code": "DS456",
        "course_name": "Project Management",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "DS211"
        ],
        "Term": "First",
        "department": "DS"
    },
    {
        "code": "DS495",
        "course_name": "Selected Topics in Operations Research and Decision Support-1",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "Passing 60 Credit Hours"
        ],
        "Term": "null",
        "department": "DS"
    },
    {
        "code": "DS496",
        "course_name": "Selected Topics in Operations Research and Decision Support-2",
        "credit_hours": 3,
        "distribution_category": "Applied_Sciences",
        "type": "Elective",
        "prerequisites": [
            "Passing 60 Credit Hours"
        ],
        "Term": "null",
        "department": "DS"
    },
    {
        "code": "DS497",
        "course_name": "Graduation_Project-1",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours"
        ],
        "Term": "All",
        "department": "DS"
    },
    {
        "code": "DS498",
        "course_name": "Graduation_Project-2",
        "credit_hours": 3,
        "distribution_category": "Graduation_Project",
        "type": "Mandatory",
        "prerequisites": [
            "Passing 85 Credit Hours",
            "DS497"
        ],
        "Term": "All",
        "department": "DS"
    }

]

# Distribution & Credit Rules
DISTRIBUTION_RULES = {
    "Bachelor's Degree Requirements in Computer and Information Sciences": {
        "total_credit_hours_required_for_graduation": 135,
        "distribution": {
            "General_Requirements - Mandatory": 6,
            "General_Requirements - Elective": 6,
            "Math_And_Basic_Sciences": 21,
            "Basic_Computer_Science": 36,
            "Applied_Sciences - Mandatory": 39,
            "Applied_Sciences - Elective": 18,
            "Graduation_Project": 6,
            "Training_Field": 3,
        }
    },
    "Credit Hour Limits": {
        "regular": [
            {"min_credit_hours": 9},
            {"condition": "GPA >= 2.0 and expected_to_graduate", "max_credit_hours": 21},
            {"condition": "GPA >= 2.0", "max_credit_hours": 18},
            {"condition": "1.0 <= GPA < 2.0", "max_credit_hours": 15},
            {"condition": "GPA <= 1.0", "max_credit_hours": 12},
        ],
        "summer": [
            {"max_credit_hours": 6},
            {"condition": "expected_to_graduate", "max_credit_hours": 9},
        ]
    }
}

def topological_sort(course_ids, all_courses):
    graph = {c['code']: set(c.get('prerequisites', [])) for c in all_courses}
    visited, temp, order = set(), set(), []

    def visit(n):
        if n in temp: return
        if n not in visited:
            temp.add(n)
            for p in graph.get(n, []):
                visit(p)
            temp.remove(n)
            visited.add(n)
            order.append(n)

    for cid in course_ids:
        visit(cid)
    return [c for c in order if c in course_ids]

def satisfies_prereq(pr, completed_ids, total_completed_hours):
    if not pr.startswith("Passing"):
        return pr in completed_ids
    m = re.search(r"Passing\s+(\d+)", pr)
    if m:
        return total_completed_hours >= int(m.group(1))
    return False

def count_locked_courses(course_id, all_remaining_courses):
    rev = defaultdict(list)
    for course in all_remaining_courses:
        for pre in course.get('prerequisites', []):
            rev[pre].append(course['code'])

    locked = set()
    def dfs(u):
        for downstream in rev.get(u, []):
            if downstream not in locked:
                locked.add(downstream)
                dfs(downstream)
    dfs(course_id)
    return len(locked)

def course_priority(course, all_remaining_courses, term):
    score = 1000 if course['type'] == 'Mandatory' else 100

    terms = course.get('Term', [])
    if terms == term:
        score += 500
    elif "All" in terms:
        score += 200
    else:
        score += 100

    score += count_locked_courses(course['code'], all_remaining_courses) * 20
    return score

def csp_select(eligible, all_remaining, credit_limit, deficits, term):
    best, best_state = [], (-1, 0)
    eligible.sort(key=lambda c: -course_priority(c, all_remaining, term))
    def score(defs, ch):
        return (sum(1 for v in defs.values() if v <= 0), ch)

    def backtrack(i, used_ch, defs, chosen):
        nonlocal best, best_state
        if used_ch > credit_limit:
            return
        s = score(defs, used_ch)
        if s > best_state:
            best_state = s
            best = chosen.copy()
        if all(v <= 0 for v in defs.values()) or i >= len(eligible):
            return

        c = eligible[i]
        ch = c['credit_hours']
        if c['distribution_category'] in ['General_Requirements', 'Applied_Sciences']:
            cat = f"{c['distribution_category']} - {c['type'].capitalize()}"
        else:
            cat = c['distribution_category']

        if defs.get(cat, 0) > 0:
            new_defs = defs.copy()
            new_defs[cat] = max(new_defs[cat] - ch, 0)
            chosen.append(c)
            backtrack(i + 1, used_ch + ch, new_defs, chosen)
            chosen.pop()
        backtrack(i + 1, used_ch, defs, chosen)

    backtrack(0, 0, deficits, [])
    sorted_ids = topological_sort([c['code'] for c in best], eligible)
    id_map = {c['code']: c for c in eligible}
    return [id_map[i] for i in sorted_ids]

def determine_credit_limit(gpa, exp, sem, rem):
    rules = DISTRIBUTION_RULES['Credit Hour Limits'][sem]
    for r in rules:
        if 'min_credit_hours' in r and rem < r['min_credit_hours']:
            return rem
        if 'condition' in r and 'max_credit_hours' in r:
            expr = r['condition'].replace('GPA', str(gpa)).replace('expected_to_graduate', str(exp))
            try:
                if eval(expr):
                    return r['max_credit_hours']
            except Exception:
                pass
        elif 'max_credit_hours' in r:
            return r['max_credit_hours']
    return 0

def compute_deficits(completed, student_department):
    req = DISTRIBUTION_RULES["Bachelor's Degree Requirements in Computer and Information Sciences"]['distribution']
    sums = {k: 0 for k in req}

    for c in completed:
        course_dept = str(c.get('department', 'null'))

        # Skip counting mandatory courses from outside the department
        if c['type'].lower() == 'mandatory' and course_dept != student_department and course_dept != 'null':
            continue

        mand_key = f"{c['distribution_category']} - {c['type'].capitalize()}"
        if mand_key in sums:
            sums[mand_key] += c['credit_hours']
        elif c['distribution_category'] in sums:
            sums[c['distribution_category']] += c['credit_hours']

    deficits = {k: max(v - sums.get(k, 0), 0) for k, v in req.items()}
    return deficits

class AcademicAdvisor:
    @staticmethod
    def determine_level(total_credits):
        """Determine the student's level."""
        if total_credits < 27:
            return "First Level"
        elif total_credits < 60:
            return "Second Level"
        elif total_credits < 96:
            return "Third Level"
        else:
            return "Fourth Level"

    def __init__(self, student_id, gpa, expected_to_graduate, semester, term, department,completed_course_details=None, all_courses=None):
        self.student_id = student_id
        self.gpa = gpa
        self.expected_to_graduate = expected_to_graduate
        self.semester = semester
        self.term = term
        self.department = department

        # ✅ Use injected data if available, otherwise fall back to fetch functions
        self.completed_courses = completed_course_details if completed_course_details is not None else fetch_completed_course_details(student_id)
        self.all_courses = all_courses if all_courses is not None else fetch_all_courses()
        self.completed_ids = [c['code'] for c in self.completed_courses]
        self.total_completed_hours = sum(c['credit_hours'] for c in self.completed_courses)
        student_level = self.determine_level(self.total_completed_hours)

        if student_level == "Fourth Level":
            self.expected_to_graduate = True

        self.remaining = self.filter_remaining_courses()
        self.eligible = self.get_eligible_courses()

        self.credit_limit = determine_credit_limit(
            self.gpa,
            self.expected_to_graduate,
            self.semester,
            sum(c['credit_hours'] for c in self.remaining)
        )
        self.deficits = compute_deficits(self.completed_courses, self.department)

        if self.department  in ['null', 'none', '']:#new
            self.eligible = [c for c in self.eligible if c['distribution_category'] != 'Applied_Sciences' or'Graduation_Project'or'Training_Field' ]

    def get_available_outside_department_courses(self):
        elective_course_codes = {c['code'] for c in self.get_eligible_courses() if c['type'] == 'Elective'}
        seen_codes = set()
        available = []

        for c in self.all_courses:
            code = c['code']
            if (
                code in self.completed_ids or
                code in elective_course_codes or
                code in seen_codes
            ):
                continue

            if self.term not in c.get('Term', []) and "All" not in c.get('Term', []):
                continue

            course_dept = str(c.get('department', 'null'))
            is_outside_dept = course_dept != self.department and course_dept != 'null'

            if not is_outside_dept:
                continue

            if c['distribution_category'] == 'Graduation_Project':
                continue
            prereqs = c.get('prerequisites', [])
            if all(satisfies_prereq(pr, self.completed_ids, self.total_completed_hours) for pr in prereqs):
                available.append(c)
                seen_codes.add(code)
        return available

    def filter_remaining_courses(self):
        return [
            c for c in self.all_courses
            if c['code'] not in self.completed_ids and
            (self.term in c.get('Term', []) or "All" in c.get('Term', [])) and
            not (
                c['distribution_category'] in ('Applied_Sciences', 'Graduation_Project') and
                str(c.get('department', 'null')) not in (self.department, 'null')
            )
        ]

    def get_eligible_courses(self):
        eligible = []
        for c in self.remaining:
            if all(satisfies_prereq(pr, self.completed_ids, self.total_completed_hours)
                   for pr in c.get('prerequisites', [])):
                eligible.append(c)
        return eligible

    def suggest_core_courses(self):
        core_eligible = [c for c in self.eligible if c['type'] != 'Elective']
        return csp_select(core_eligible, self.remaining, self.credit_limit, self.deficits, self.term)

    def suggest_electives(self, remaining_credit_capacity):
        result = {}

        def pick_elective(category, course_credits, min_slots, available_credits):
            deficit = self.deficits.get(f"{category} - Elective", 0)
            possible_slots = max(deficit // course_credits, min_slots)
            credit_based_slots = available_credits // course_credits
            slots = min(possible_slots, credit_based_slots)
            options = [
                c for c in self.eligible
                if c['distribution_category'] == category and c['type'] == 'Elective'
            ]
            return slots, options, slots * course_credits

        general_slots, general_options, used_credits_general = pick_elective(
            'General_Requirements', 2, 0, remaining_credit_capacity)
        applied_remaining_credit = remaining_credit_capacity - used_credits_general
        applied_slots, applied_options, used_credits_applied = pick_elective(
            'Applied_Sciences', 3, 0, applied_remaining_credit)

        result['General'] = general_slots
        result['GeneralOptions'] = general_options
        result['Applied'] = applied_slots
        result['AppliedOptions'] = applied_options
        result['TotalElectives'] = general_slots + applied_slots
        result['UsedElectiveCredits'] = used_credits_general + used_credits_applied

        return result

    def run(self):
        result = {}
        student_level = self.determine_level(self.total_completed_hours)
        TOTAL_CREDIT_HOURS = 135
        total_remaining = TOTAL_CREDIT_HOURS - self.total_completed_hours

        # 1. core_courses as list of codes
        core_courses = self.suggest_core_courses()
        result['core_courses'] = [c['code'] for c in core_courses]

        # 2. distribution (same structure, but keep only relevant keys)
        distribution = {}
        for c in self.remaining:
            category = c['distribution_category']
            distribution[category] = distribution.get(category, 0) + c['credit_hours']
        result['distribution'] = distribution

        # 3. electives (convert options to code lists)
        electives = self.suggest_electives(self.credit_limit - sum(c['credit_hours'] for c in core_courses))
        result['electives'] = {
            'Applied': electives.get('Applied', 0),
            'AppliedOptions': [c['code'] for c in electives.get('AppliedOptions', [])],
            'General': electives.get('General', 0),
            'GeneralOptions': [c['code'] for c in electives.get('GeneralOptions', [])],
            'TotalElectives': electives.get('TotalElectives', 0),
            'UsedElectiveCredits': electives.get('UsedElectiveCredits', 0)
        }

        # 4. ineligible_courses (use CourseCode and MissingPrereqs)
        remaining_mandatory = [c for c in self.remaining if c['type'] == 'Mandatory' and c not in self.eligible]
        ineligible_courses = []
        for c in remaining_mandatory:
            missing_prereqs = [pr for pr in c.get('prerequisites', []) if not satisfies_prereq(pr, self.completed_ids, self.total_completed_hours)]
            if missing_prereqs:
                ineligible_courses.append({
                    'CourseCode': c['code'],
                    'MissingPrereqs': ', '.join(missing_prereqs)
                })
        result['ineligible_courses'] = ineligible_courses

        # 5. outside_dept (use AvailableOutside as list of codes, and rename keys)
        if self.department not in ['null', 'none', '']:
            outside_dept_completed = [
                c for c in self.completed_courses
                if str(c.get('department', 'null')) != self.department and str(c.get('department', 'null')) != 'null'
            ]
            num_outside_dept_taken = len(outside_dept_completed)
            can_take_outside = max(0, 2 - num_outside_dept_taken)
            available_outside = self.get_available_outside_department_courses()
            result['outside_dept'] = {
                'AvailableOutside': [c['code'] for c in available_outside],
                'CanTakeOutside': can_take_outside ,  
                'NumOutsideDeptTaken': num_outside_dept_taken  
            }

        # 6. remaining_requirements (flatten keys and match names)
        remaining_requirements = {}
        for category, deficit in self.deficits.items():
            if 'Graduation_Project' in category:
                key = 'Graduation Project'
            elif 'Total' in category:
                key = 'Total Credits Remaining'
            else:
                key = category.replace('_', ' ')
            if deficit > 0:
                remaining_requirements[key] = f"{deficit} credits remaining"
            else:
                remaining_requirements[key] = "Fulfilled"
        # Add total credits remaining
        remaining_requirements['Total Credits Remaining'] = str(total_remaining)
        result['remaining_requirements'] = remaining_requirements

        # 7. student_summary (match key names)
        result['student_summary'] = {
            'AcademicLevel': student_level,
            'CreditLimit': self.credit_limit,
            'CurrentTerm': self.term,
            'Department': self.department,
            'Gpa': self.gpa,
            'StudentId': self.student_id,
            'TotalCompletedHours': self.total_completed_hours,
            'TotalRemaining': total_remaining
        }

        # 8. total_core_credits (same as before)
        result['total_core_credits'] = sum(c['credit_hours'] for c in core_courses)

        return result