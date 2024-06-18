import api from "@/pages/untils/request.js"
var base='/business';
export default{
	GetSelectInfo() {
	  return api.request({
	    url: base+'/Select/examlist',
	    method: 'get'
	  })
	},
	GetChapeterUnInfo(data) {
	  return api.request({
	    url: base+'/Chapeter/GetChapeterUnInfo?id='+data,
	    method: 'post'
	  })
	},
	ExamSelect() {
	  return api.request({
	    url: base+'/Select/examlist',
	    method: 'get'
	  })
	}
}