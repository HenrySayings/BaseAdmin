<!--
 * @Descripttion: (试卷表/Exam)
 * @Author: (admin)
 * @Date: (2024-05-11)
-->
<template>
  <div>
    <el-form :model="queryParams" label-position="right" inline ref="queryRef" v-show="showSearch" @submit.prevent>
      <el-form-item>
        <el-button icon="refresh" @click="resetQuery">{{ $t('btn.reset') }}</el-button>
      </el-form-item>
    </el-form>
    <!-- 工具区域 -->
    <el-row :gutter="15" class="mb10">
      <el-col :span="1.5">
        <el-button type="primary" v-hasPermi="['exam:add']" plain icon="plus" @click="handleAdd">
          {{ $t('btn.add') }}
        </el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList" :columns="columns"></right-toolbar>
    </el-row>

    <el-table
      :data="dataList"
      v-loading="loading"
      ref="table"
      border
      header-cell-class-name="el-table-header-cell"
      highlight-current-row
      @sort-change="sortChange">
      <el-table-column prop="examName" label="考试名称" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('examName')" />
      <el-table-column prop="courseName" label="课程" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('course')" />
      <el-table-column prop="startTime" label="开始时间" :show-overflow-tooltip="true" v-if="columns.showColumn('startTime')" />
      <el-table-column prop="endTime" label="结束时间" :show-overflow-tooltip="true" v-if="columns.showColumn('endTime')" />
      <el-table-column prop="duration" label="考试时长" align="center" v-if="columns.showColumn('duration')" />
      <el-table-column prop="createUser" label="创建人" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('createUser')" />
      <el-table-column prop="createTime" label="创建时间" :show-overflow-tooltip="true" v-if="columns.showColumn('createTime')" />
      <el-table-column label="操作" width="160">
        <template #default="scope">
          <el-button type="success" size="small" icon="edit" title="编辑" v-hasPermi="['exam:edit']" @click="handleUpdate(scope.row)"></el-button>
          <el-button
            type="danger"
            size="small"
            icon="delete"
            title="删除"
            v-hasPermi="['exam:delete']"
            @click="handleDelete(scope.row)"></el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize" @pagination="getList" />

    <!-- 添加或修改试卷表对话框 -->
    <el-dialog :title="title" :lock-scroll="false" v-model="open">
      <el-form ref="formRef" :model="form" :rules="rules" label-width="100px">
        <el-row :gutter="20">
          <el-col :lg="12">
            <el-form-item label="考试名称" prop="examName">
              <el-input v-model="form.examName" placeholder="请输入考试名称" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="课程" prop="course">
              <el-select v-model="form.courseId" placeholder="请选择课程">
                <el-option v-for="item in optionsBy" :key="item.courseId" :label="item.courseName" :value="item.courseId" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :lg="12">
            <el-form-item label="开始时间:" prop="startTime">
              <el-date-picker v-model="form.startTime" type="datetime" :teleported="false" placeholder="选择日期时间"></el-date-picker>
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="结束时间:" prop="endTime">
              <el-date-picker v-model="form.endTime" type="datetime" :teleported="false" placeholder="选择日期时间"></el-date-picker>
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="考试时长:" prop="duration">
              <el-input type="number" v-model.number="form.duration" placeholder="请输入考试时长" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer v-if="opertype != 3">
        <el-button text @click="cancel">{{ $t('btn.cancel') }}</el-button>
        <el-button type="primary" @click="submitForm">{{ $t('btn.submit') }}</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup name="exam">
import { listExam, addExam, delExam, updateExam, getExam } from '@/api/business/exam.js'
import { getselect } from '@/api/business/courseservice.js'
const { proxy } = getCurrentInstance()
const ids = ref([])
const loading = ref(false)
const showSearch = ref(true)
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  sort: '',
  sortType: 'asc'
})
const columns = ref([
  { visible: true, prop: 'examId', label: 'ExamId' },
  { visible: true, prop: 'examName', label: 'ExamName' },
  { visible: true, prop: 'course', label: 'Course' },
  { visible: true, prop: 'courseId', label: 'CourseId' },
  { visible: true, prop: 'startTime', label: 'StartTime' },
  { visible: true, prop: 'endTime', label: 'EndTime' },
  { visible: true, prop: 'duration', label: 'Duration' },
  { visible: true, prop: 'isEnable', label: 'IsEnable' },
  { visible: false, prop: 'isFinish', label: 'IsFinish' },
  { visible: false, prop: 'courseName', label: 'courseName' },
  { visible: false, prop: 'judgeScore', label: 'JudgeScore' },
  { visible: false, prop: 'singleScore', label: 'SingleScore' },
  { visible: false, prop: 'selectScore', label: 'SelectScore' },
  { visible: false, prop: 'createUser', label: 'CreateUser' },
  { visible: false, prop: 'createTime', label: 'CreateTime' },
  { visible: false, prop: 'updateUser', label: 'UpdateUser' },
  { visible: false, prop: 'updateTime', label: 'UpdateTime' },
  { visible: false, prop: 'isDeleted', label: 'IsDeleted' }
])
const total = ref(0)
const dataList = ref([])
const queryRef = ref()
const defaultTime = ref([new Date(2000, 1, 1, 0, 0, 0), new Date(2000, 2, 1, 23, 59, 59)])
const optionsBy = ref([])
function OptionsByCourse() {
  loading.value = true
  getselect()
    .then((res) => {
      const { code, data } = res
      if (code === 200) {
        optionsBy.value = data
      }
      loading.value = false
    })
    .catch((error) => {
      console.error('Error fetching options:', error)
      loading.value = false
    })
}
var dictParams = []

function getList() {
  loading.value = true
  listExam(queryParams).then((res) => {
    const { code, data } = res
    if (code == 200) {
      dataList.value = data.result
      total.value = data.totalNum
      loading.value = false
    }
  })
}

// 查询
function handleQuery() {
  queryParams.pageNum = 1
  getList()
}

// 重置查询操作
function resetQuery() {
  proxy.resetForm('queryRef')
  handleQuery()
}
// 自定义排序
function sortChange(column) {
  var sort = undefined
  var sortType = undefined

  if (column.prop != null && column.order != null) {
    sort = column.prop
    sortType = column.order
  }
  queryParams.sort = sort
  queryParams.sortType = sortType
  handleQuery()
}

/*************** form操作 ***************/
const formRef = ref()
const title = ref('')
// 操作类型 1、add 2、edit 3、view
const opertype = ref(0)
const open = ref(false)
const state = reactive({
  single: true,
  multiple: true,
  form: {},
  rules: {},
  options: {
    // IsEnable 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
    isEnableOptions: []
  }
})

const { form, rules, options, single, multiple } = toRefs(state)

// 关闭dialog
function cancel() {
  open.value = false
  reset()
}

// 重置表单
function reset() {
  form.value = {
    examId: null,
    examName: null,
    course: null,
    courseId: null,
    startTime: null,
    endTime: null,
    duration: null,
    isEnable: null,
    isFinish: null,
    examScore: null,
    judgeScore: null,
    singleScore: null,
    selectScore: null,
    createUser: null,
    createTime: null,
    updateUser: null,
    updateTime: null,
    isDeleted: null
  }
  proxy.resetForm('formRef')
}

// 添加按钮操作
function handleAdd() {
  reset()
  open.value = true
  title.value = '添加考试'
  opertype.value = 1
}
// 修改按钮操作
function handleUpdate(row) {
  reset()
  const id = row.id || ids.value
  getExam(id).then((res) => {
    const { code, data } = res
    if (code == 200) {
      open.value = true
      title.value = '修改考试'
      opertype.value = 2

      form.value = {
        ...data
      }
    }
  })
}

// 添加&修改 表单提交
function submitForm() {
  proxy.$refs['formRef'].validate((valid) => {
    if (valid) {
      if (form.value.startTime > form.value.endTime) {
        proxy.$modal.msgSuccess('开始时间不能大于结束时间')
        return
      }
      if (form.value.id != undefined && opertype.value === 2) {
        updateExam(form.value).then((res) => {
          proxy.$modal.msgSuccess('修改成功')
          open.value = false
          getList()
        })
      } else {
        addExam(form.value).then((res) => {
          proxy.$modal.msgSuccess('新增成功')
          open.value = false
          getList()
        })
      }
    }
  })
}

// 删除按钮操作
function handleDelete(row) {
  const Ids = row.id || ids.value

  proxy
    .$confirm('是否确认删除参数编号为"' + Ids + '"的数据项？', '警告', {
      confirmButtonText: proxy.$t('common.ok'),
      cancelButtonText: proxy.$t('common.cancel'),
      type: 'warning'
    })
    .then(function () {
      return delExam(Ids)
    })
    .then(() => {
      getList()
      proxy.$modal.msgSuccess('删除成功')
    })
}
onMounted(() => {
  OptionsByCourse()
})
handleQuery()
</script>
