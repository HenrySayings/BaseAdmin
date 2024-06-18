<style>
	.container{
		width: 100vw;
		height: 100vh;
	}
	.video{
		width: 100vw;
		height: 30vh;
	}
	video{
		width: 100vw;
		height: 30vh;
	}
	.tab{
		display: flex;
		justify-content: space-between;
		padding:25rpx 30rpx;
		color: #858487;
		box-shadow: 0rpx 3rpx 8rpx 5rpx #f9f9f9;
		font-size: 30rpx;
	}
	.title{
		padding: 30rpx 0rpx;
		margin: 0rpx 30rpx;
		background-color: white;
		margin-top: 10rpx;
		border-bottom: 1rpx solid #f1f1f1;
		
	}
	.title view:nth-of-type(1){
		font-size: 40rpx;
		font-weight: bold;
		margin-bottom: 20rpx;
	}
	.title view:nth-of-type(2){
		font-size: 28rpx;
		color: #808080;
	}
	.title view:nth-of-type(2) text{
		margin:0rpx 10rpx 0rpx 20rpx;
		display: inline-block;
		margin-top: 10rpx;
	}
	.lessons{
		padding: 30rpx;
	}
	.lessons_title{
		font-size: 33rpx;
		font-weight: 700;
		padding: 20rpx 0rpx;
	}
	.lessons_content{
		font-size: 30rpx;
		color: #444444;
	}
	.lessons_content view{
		padding: 10rpx 0rpx;
	}
	.icon-daima{
		margin: 0rpx 20rpx;
	}
</style>
<template>
	<view class="container">
			<view class="video">
				<video :show-mute-btn="show_mute_btn" 
				:enable-play-gesture="enable_play_gesture" 
				:show-center-play-btn="show_center_play_btn" 
				:page-gesture="page_gesture"
				 :src="fileurl" controls></video>
			</view>
		<view class="content">
			<view class="tab">
				<text>章节</text>
				<text>详情</text>
				<text>评论</text>
				<text>收藏</text>
				<text>下载</text>
			</view>
			<view class="title">
				<view class="">{{title}}</view>
				<view class="icon">入门 <text class="iconfont icon-14rentou"></text>213241</view>
			</view>
			<view class="lessons" v-for="i in chapeterItem">
				<view class="lessons_item">
					<view class="lessons_title">
						{{i.chapeterName}}
					</view>
					<view class="lessons_content" v-for="y in i.chapeterItemsName">
						<text type="" class="icon-daima" @click="goToLesson(y.fileUrl)" >{{y.chapeterItemName}}</text> 
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import req from "../../../pages/apis/select.js";
	export default {
    data() {
        return {
           page_gesture:true,
		   show_center_play_btn:false,
		   enable_play_gesture:true,
		   show_mute_btn:true,
		   fileurl:"",
		   title: '', // 初始为空，将通过API填充
		chapeterItem: [] // 初始为空数组，将通过API填充
        }
    },
    methods: {
       goToLesson(value){
		   this.fileurl=value;
	   }
    },
	onLoad(option) {
		req.GetChapeterUnInfo(option.id).then(res=>{
				this.title=res.data.title;
				this.chapeterItem=res.data.chapeterItem;
		});
	}
}
</script>
