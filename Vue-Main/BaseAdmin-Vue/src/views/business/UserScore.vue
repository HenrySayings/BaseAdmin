<!--
 * @Descripttion: (成绩表/UserScore)
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
      <el-table-column prop="name" label="用户名称" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('name')" />
      <el-table-column prop="score" label="成绩" align="center" v-if="columns.showColumn('score')" />
      <el-table-column prop="exams" label="考试类型" align="center" v-if="columns.showColumn('exams')" />
      <el-table-column prop="dateTime" label="提交时间" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('dateTime')" />
      <el-table-column label="操作" width="160">
        <template #default="scope">
          <el-button
            type="success"
            size="small"
            icon="edit"
            title="编辑"
            v-hasPermi="['userscore:edit']"
            @click="handleUpdate(scope.row)"></el-button>
          <el-button
            type="danger"
            size="small"
            icon="delete"
            title="删除"
            v-hasPermi="['userscore:delete']"
            @click="handleDelete(scope.row)"></el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize" @pagination="getList" />

    <!-- 添加或修改成绩表对话框 -->
    <el-dialog :title="title" :lock-scroll="false" v-model="open">
      <el-form ref="formRef" :model="form" :rules="rules" label-width="100px">
        <el-row :gutter="20">
          <el-col :lg="12">
            <el-form-item label="UserId" prop="userId">
              <el-input v-model.number="form.userId" placeholder="请输入UserId" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="User" prop="user">
              <el-input v-model="form.user" placeholder="请输入User" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="Course" prop="course">
              <el-input v-model="form.course" placeholder="请输入Course" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="CourseId" prop="courseId">
              <el-input v-model.number="form.courseId" placeholder="请输入CourseId" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="ExamId" prop="examId">
              <el-input v-model.number="form.examId" placeholder="请输入ExamId" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="Score" prop="score">
              <el-input v-model="form.score" placeholder="请输入Score" />
            </el-form-item>
          </el-col>
          <el-col :lg="12">
            <el-form-item label="IsDeleted" prop="isDeleted">
              <el-radio-group v-model="form.isDeleted">
                <el-radio v-for="item in options.isDeletedOptions" :key="item.dictValue" :label="item.dictValue">
                  {{ item.dictLabel }}
                </el-radio>
              </el-radio-group>
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

<script setup name="userscore">
import { listUserScore, addUserScore, delUserScore, updateUserScore, getUserScore } from '@/api/business/userscore.js'
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
  { visible: true, prop: 'id', label: 'Id' },
  { visible: true, prop: 'userId', label: 'UserId' },
  { visible: true, prop: 'user', label: 'User' },
  { visible: true, prop: 'course', label: 'Course' },
  { visible: true, prop: 'courseId', label: 'CourseId' },
  { visible: true, prop: 'name', label: 'name' },
  { visible: true, prop: 'score', label: 'Score' },
  { visible: true, prop: 'createUser', label: 'CreateUser' },
  { visible: false, prop: 'updateUser', label: 'UpdateUser' },
  { visible: false, prop: 'updateTime', label: 'UpdateTime' },
  { visible: false, prop: 'isDeleted', label: 'IsDeleted' },
  { visible: true, prop: 'dateTime', label: 'DateTime' },
  { visible: true, prop: 'exams', label: 'Exams' }
])
const total = ref(0)
const dataList = ref([])
const queryRef = ref()
const defaultTime = ref([new Date(2000, 1, 1, 0, 0, 0), new Date(2000, 2, 1, 23, 59, 59)])

var dictParams = []

function getList() {
  loading.value = true
  listUserScore(queryParams).then((res) => {
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
    // IsDeleted 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
    isDeletedOptions: []
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
    id: null,
    userId: null,
    user: null,
    course: null,
    courseId: null,
    dateTime: null,
    score: null,
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
  title.value = '添加成绩表'
  opertype.value = 1
}
// 修改按钮操作
function handleUpdate(row) {
  reset()
  const id = row.id || ids.value
  getUserScore(id).then((res) => {
    const { code, data } = res
    if (code == 200) {
      open.value = true
      title.value = '修改成绩表'
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
      if (form.value.id != undefined && opertype.value === 2) {
        updateUserScore(form.value).then((res) => {
          proxy.$modal.msgSuccess('修改成功')
          open.value = false
          getList()
        })
      } else {
        addUserScore(form.value).then((res) => {
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
      return delUserScore(Ids)
    })
    .then(() => {
      getList()
      proxy.$modal.msgSuccess('删除成功')
    })
}

handleQuery()
</script>
