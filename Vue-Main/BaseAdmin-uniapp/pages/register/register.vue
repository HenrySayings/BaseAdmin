<style>
	.container{
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
	}
	.container image{
		width: 200rpx;
		height: 200rpx;
		margin-top: 20%;
		border-radius: 50%;
	}
	.phone_number,
	.verification_code{
		width: 560rpx;
		height: 88rpx;
		margin-top: 50rpx;
		background-color: rgb(238, 238, 238);
		padding-left: 30rpx;
		border-radius: 50rpx;
		box-shadow: 2rpx 2rpx 10rpx 0rpx #dfdee3;
	}
	
	.btn{
		margin-top: 50rpx;
		height: 88rpx;
		width: 583rpx;
		border-radius: 50rpx;
		background-color: rgb(5, 187, 29);
		text-align: center;
		line-height: 88rpx;
		color: white;
		box-shadow: 0rpx 5rpx 10rpx #a2a2a2;
	}
	.agree{
		width: 560rpx;
		text-align: center;
		display: flex;
		justify-content: center;
		color: #999999;
		margin-top: 40rpx;
		font-size: 28rpx;
		align-items: center;
	}
	.agree radio{
		transform: scale(.7);
	}
	.agree a{
		color: #858188;
	}
	.verification_code_father{
		position: relative;
	}
	.get_dode{
		position: absolute;
		right: 38rpx;
		bottom: 23rpx;
		color: #808080;
		font-size: 29rpx;
		padding-left: 15rpx;
		height: 44rpx;
		line-height: 50rpx;
		border-left: 2rpx solid #a7a7a7;
		width: 147rpx;
		z-index: 2;
		text-align: center;
	}
	.other_login{
		font-size: 28rpx;
		height: 100rpx;
		line-height: 100rpx;
		margin-top: 50rpx;
		position: relative;
		padding: 0rpx 10rpx;
		color: rgb(126, 126, 126);
	}
	.other_login::after{
		content: "";
		position: absolute;
		height: 2rpx;
		width: 170rpx;
		background-color: #CCCCCC;
		margin-top: 50rpx;
		left: 218rpx;
	}
	.other_login::before{
		content: "";
		position: absolute;
		height: 2rpx;
		width: 170rpx;
		background-color: #CCCCCC;
		margin-top: 50rpx;
		right: 218rpx;
	}
	.icon-weixin{
		font-family: 'iconfont';
		font-size: 88rpx;
		color: #999999;
	}
	.phClass{
		font-size: 28rpx;
	}
	.icon-weixin{
		font-family: 'iconfont';
		font-size: 68rpx;
		color: #444444 !important;
		position: relative;
	}
	button{
		width: 72rpx;
		height: 75rpx;
		position: absolute;
		bottom: 0rpx;
		opacity: 0;
	}
</style>
<template>
	<view class="container">
		<image src="../../static/face.jpg" mode=""></image>
		<view>
			<input v-model="Logininput.username" placeholder-class="phClass" maxlength="14" class="phone_number" type="input"
			 placeholder="请输入账户" />
		</view>
		<view>
			<input v-model="Logininput.password" placeholder-class="phClass" maxlength="14" class="phone_number" type="password"
			 placeholder="请输入密码" />
		</view>
		<view @click="login" class="btn">登录</view>
		<view class="agree">
			<radio @click="change" :checked="is_true" />
			同意<a href="">《使用条款和数据隐私政策》</a>
		</view>
		<text class="other_login">其它账号登录</text>
		<view class="icon-weixin">
			<button @getuserinfo="getUserInfo" bind open-type="getUserInfo" withCredentials="true">获取用户信息</button>
		</view>
	</view>
</template>
<script>
	import req from "../apis/index.js"
	export default {
		data() {
			return {
				is_true: true,
				Logininput:{
					username:"",
					password:""
				},
				//用户输入得验证码
				verification_code: null,
				verify: "获取验证码",
				//验证倒计时
				count_down: 60,
				//存储接收验证码
				verify_code: 123,
				//获取验证码是否禁用
				is_click: true,
				status:""
			}
		},
		onLoad: function() {
			
		},
		methods: {
			change() {
				this.is_true = !this.is_true
			},
			 getCode() {
			  getCodeImg().then((res) => {
			    codeUrl.value = 'data:image/gif;base64,' + res.data.img
			    loginForm.value.uuid = res.data.uuid
			    captchaOnOff.value = res.data.captchaOff
			  })
			},
			get_code() {
				var phone_reg = /^[1][3,4,5,7,8,9][0-9]{9}$/;
				if (!phone_reg.test(this.phone_number)) {
					uni.showToast({
						"title": "手机号错误，请重新填写",
						"icon": "none"
					});
					return;
				}
				let that = this;
				that.is_click = false;
				var timer = setInterval(function() {
					that.verify = that.count_down-- + " S";
					if (that.count_down < -1) {
						clearInterval(timer);
						that.verify = "获取验证码";
						that.count_down = 60;
						that.is_click = true;
						return;
					}
				}, 1000)
				uni.request({
					url: '',
					method: 'GET',
					data: {},
					success: res => {
						that.verify_code = res;
					},
				});
			},
			login() {
				var that = this;
				if (that.is_true == false) {
					uni.showToast({
						"title": "请同意用户协议",
						"icon": "none"
					});
					return;
				}
				const data={
					username:"",
					password:""
				}
				data.username=this.Logininput.username;
				data.password=this.Logininput.password;
				req.Login(data).then(res=>{
					if (res.code === 200) {
					  uni.setStorageSync("token", res.data);
					  uni.showToast({
					    title: '登录成功', // 提示的内容
					    icon: 'success', // 图标，可以是 'success' 或 'none'
					    duration: 2000 // 延迟时间，单位为毫秒
					  });
					  // 设置一个2秒的延迟执行跳转
					  setTimeout(function() {
					    uni.switchTab({
					      url: '/pages/index/index'
					    });
					  }, 2000); // 2000毫秒后执行跳转
					}else{
						
					}
				});
				
			}
		}
	}
</script>
