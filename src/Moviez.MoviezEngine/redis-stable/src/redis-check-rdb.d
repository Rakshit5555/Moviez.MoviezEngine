redis-check-rdb.o: redis-check-rdb.c mt19937-64.h server.h fmacros.h \
  config.h solarisfixes.h rio.h sds.h connection.h ae.h monotonic.h \
  atomicvar.h commands.h ../deps/lua/src/lua.h ../deps/lua/src/luaconf.h \
  mstr.h ebuckets.h rax.h dict.h kvstore.h adlist.h zmalloc.h anet.h \
  version.h util.h latency.h sparkline.h quicklist.h redismodule.h \
  zipmap.h ziplist.h sha1.h endianconv.h crc64.h stream.h listpack.h \
  rdb.h