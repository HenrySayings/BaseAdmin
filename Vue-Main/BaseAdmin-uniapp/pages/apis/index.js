import api from "@/pages/untils/request.js"
// 水印图片
export default{
	GetInfo() {
	  return api.request({
	    url: '/getInfo',
	    method: 'get'
	  })
	},
	Login(data) {
	    return api.request({
	        method: 'post',
	        url: '/login',
	        data: data
	    })
	},
	 logout() {
	   return api.request({
	     url: '/LogOut',
	     method: 'POST'
	   })
	 }
}
