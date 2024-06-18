import api from "@/pages/untils/request.js"
var base='/business';
export default{
	Examcommit(data) {
	  return api.request({
	    url: base+'/UserScore',
	    method: 'Post',
		data:data
	  })
	}
}