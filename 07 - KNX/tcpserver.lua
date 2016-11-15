-- init server handler
if not ready then
  require('copas')
 
  -- list of client sockets
  clients = {}
 
  -- incoming data handler
  function datahandler(sock, data)
    local ip, port
    ip, port = sock:getpeername()
    alert('[server] data from %s:%d - %s', ip, port, data)
   
    
    -- send reply
    if data == 'HELLO' then
      sock:send('HELLO TO YOU! \r\n')
    end
    
    
     -- hbt  send write 1 to group
    if data == 'ON' then
      sock:send('write on group\n')
    	grp.write('4/0/0', true)
    end
    
    if data == 'OFF' then
      grp.write('4/0/0', false)
      sock:send('write on group\n')
    end
    
    alert(data)
    if string.match(data, "alex") then
      alert('alex matched')
      kxnAlex(sock,data)
    else
      knxHein(sock,data)
      alert('alex not matched')
    end
    
  end
  
  function knxAlex(sock,data)
    if string.match(data, "set") then
      alert('set data')
    else
      alert('do not set data')
    end
  end
  
  function knxHein(sock,data)
    -- send reply
    if data == 'blij' then
      sock:send('framblij /r/n')
    end
    
    if data == 'boos' then
      sock:send('framboos /r/n')
    end
  end
  
 -- connection handler
  function connhandler(sock)
    -- enable keep-alive to check for disconnect events
    sock:setoption('keepalive', true)
 
    local ip, port, data, err, id
 
    -- get ip and port from socket
    ip, port = sock:getpeername()
 
    -- client id
    id = string.format('%s:%d', ip, port)
 
    alert('[server] connection from %s', id)
 
    -- save socket reference
    clients[ id ] = sock
 
    -- main reader loop
    while true do
      -- wait for single line of data (until \n, \r is ignored)
      data, err = copas.receive(sock, '*l')
  
    
      -- error while receiving
      if err then
        alert('[server] closed connection from %s:%d', ip, port)
        -- remove socket reference
        clients[ id ] = nil
        return
      end
 
      -- handle data frame
      datahandler(sock, data)
    end
  end
 
  -- bind to port 12345
  tcpserver = socket.bind('*', 123459)
 
  -- error while binding, try again later
  if not tcpserver then
    os.sleep(5)
    error('[server] error: cannot bind')
  end

  -- set server connection handler
  copas.addserver(tcpserver, connhandler)
 
  -- create local udp server on port 23456
  udpserver = socket.udp()
  udpserver:setsockname('127.0.0.1', 23456)
 udpserver:settimeout(0.1)
 
  ready = true
end
 
-- perform server tasks for one second (5 x (0.1 + 0.1))
for i = 1, 5 do
  message = udpserver:receive()
 
  -- got message from udp, send to all active clients
if message then
   for id, sock in pairs(clients) do
      sock:send(message .. '\r\n')
   end
 end
 
  copas.step(0.1)
end