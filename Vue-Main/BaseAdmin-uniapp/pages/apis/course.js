import api from "@/pages/untils/request.js"
var base='/business';
export default{
	GetUniCourses() {
	  return api.request({
	    url: base+'/CourseService/GetUniCourses',
	    method: 'get'
	  })
	}
}