export default {
    config: {
        baseURL: 'http://localhost:8888',
        getToken() {
            let token = uni.getStorageSync('token');
            if (!token) {
                return uni.switchTab({
                    url: '/pages/register/register'
                })
            }
            return token
        },
        // 请求拦截器
        beforeRequest(options = {}) {
            return new Promise((resolve, reject) => {
                options.url = this.baseURL + options.url;
                options.method = options.method || 'GET';
                options.header = {
                    "Authorization": "Bearer " + this.getToken()
                }
                resolve(options)
            })
        },
        // 响应拦截器
        handleResponse(data) {
            return new Promise((resolve, reject) => {
                const [err, res] = data;
				if(res.data.code === 401){
					uni.redirectTo({
					    url: '/pages/register/register'
					})
				}
				if(res.data.code === 210){
					uni.showToast({
					  title: '暂无数据',
					  icon: 'fail',
					  duration: 1000 // 显示时间，单位ms
					});
					setTimeout(() => {
					    // 1秒后返回上一级页面
					    uni.navigateBack();
					  }, 2000);
				}
                if (res && res.data.code !== 200) {
                    let msg = res.data.msg || '请求错误';
                    uni.showToast({
                        icon: 'none',
                        title: msg
                    })
                    return reject(msg)
                }
				
                if (err) {
                    uni.showToast({
                        icon: 'none',
                        title: '请求错误'
                    })
                    return reject(err)
                }
                return resolve(res.data)
            })
        },
    },
    request(options = {}) {
        return this.config.beforeRequest(options).then(opt => {
            return uni.request(opt)
        }).then(res => this.config.handleResponse(res))
    }
}
